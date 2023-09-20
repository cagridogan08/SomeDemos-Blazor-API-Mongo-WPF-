using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using InMemoryEventBus.Handlers;
using InMemoryEventBus.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InMemoryEventBus.Consumer
{
    public class EventConsumer<T> : IConsumer<T>
    {
        #region Constructor

        private readonly ChannelReader<Event<T>> _channelReader;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<EventConsumer<T>> _logger;
        private CancellationTokenSource? _cancellationTokenSource;
        public EventConsumer(ChannelReader<Event<T>> channelReader, IServiceScopeFactory serviceScopeFactory, ILogger<EventConsumer<T>> logger)
        {
            _channelReader = channelReader;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        #endregion

        public async ValueTask StartAsync(CancellationToken token = default)
        {
            CreateCancellationToken();

            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var handlers = scope.ServiceProvider.GetServices<IEventHandler<T>>().ToList();
            var contextAccessor = scope.ServiceProvider.GetService<IEventContextAccessor<T>>();

            if (handlers.FirstOrDefault() is null)
            {
                _logger.LogDebug($"No handler defined for event of {typeof(T).Name}");
                return;
            }

            if (_cancellationTokenSource != null)
                Task.Run(
                   async () => { await StartProcessing(handlers, contextAccessor, token).ConfigureAwait(false); },
                   _cancellationTokenSource.Token).ConfigureAwait(false);
        }

        private async Task StartProcessing(ICollection<IEventHandler<T>> handlers, IEventContextAccessor<T>? contextAccessor, CancellationToken token)
        {
            if (contextAccessor is null)
                throw new ArgumentNullException(nameof(contextAccessor));

            var channelIterator = _channelReader.ReadAllAsync(_cancellationTokenSource!.Token)
                .WithCancellation(_cancellationTokenSource.Token).ConfigureAwait(false);
            await foreach (var task in channelIterator)
            {
                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    break;
                }

                await Parallel.ForEachAsync(handlers, _cancellationTokenSource.Token,
                        async (handler, scopedToken) =>
                            await ExecuteHandler(handler, task, contextAccessor, scopedToken))
                    .ConfigureAwait(false);
            }
        }

        private ValueTask ExecuteHandler(IEventHandler<T> handler, Event<T> task, IEventContextAccessor<T> contextAccessor, CancellationToken scopedToken)
        {
            contextAccessor.Set(task);
            using var logScope = _logger.BeginScope(task.Metadata ?? new EventMetadata(Guid.NewGuid().ToString()));
            Task.Run(async () => await handler.HandleAsync(task.Data, scopedToken), scopedToken
            ).ConfigureAwait(false);

            return ValueTask.CompletedTask;
        }

        public async ValueTask StopAsync(CancellationToken token = default)
        {
            await DisposeAsync().ConfigureAwait(false);
        }

        private void CreateCancellationToken()
        {
            if (_cancellationTokenSource is { IsCancellationRequested: false })
                _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
        }
        public async ValueTask DisposeAsync()
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}
