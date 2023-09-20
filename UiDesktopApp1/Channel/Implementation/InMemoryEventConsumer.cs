using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UiDesktopApp1.Channel.Implementation
{
    internal class InMemoryEventConsumer<T>:IConsumer<T>
    {
        private readonly ChannelReader<Event<T>> _reader;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<InMemoryEventConsumer<T>> _logger;
        private CancellationTokenSource? _stoppingToken;

        public InMemoryEventConsumer(ChannelReader<Event<T>> reader, IServiceScopeFactory serviceScopeFactory, ILogger<InMemoryEventConsumer<T>> logger)
        {
            _reader = reader;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async ValueTask DisposeAsync()
        { 
            _stoppingToken?.Cancel();
        }

        public async ValueTask Start(CancellationToken token = default)
        {
            EnsureStoppingTokenIsCreated();
            await using var scope = _serviceScopeFactory.CreateAsyncScope();

            var handlers = scope.ServiceProvider.GetServices<IEventHandler<T>>();
            var contextAccessor = scope.ServiceProvider.GetService<IEventContextAccessor<T>>();

            var eventHandlers = handlers.ToList();
            if (eventHandlers.FirstOrDefault() is null)
            {
                _logger.LogDebug($"No handler defined for event of {typeof(T).Name}");
            }

            await Task.Run(async () =>
                {
                    if (contextAccessor != null) await StartProcessing(eventHandlers, contextAccessor).ConfigureAwait(false);
                },
                _stoppingToken!.Token
            ).ConfigureAwait(false);

        }

        private async ValueTask StartProcessing(IEnumerable<IEventHandler<T>> handlers, IEventContextAccessor<T> contextAccessor)
        {
            var continuousChannelIterator = _reader.ReadAllAsync(_stoppingToken!.Token)
                .WithCancellation(_stoppingToken.Token)
                .ConfigureAwait(false);

            await foreach (var task in continuousChannelIterator)
            {
                if (_stoppingToken.IsCancellationRequested)
                    break;

                // invoke handlers in parallel
                await Parallel.ForEachAsync(handlers, _stoppingToken.Token,
                    async (handler, scopedToken) => await ExecuteHandler(handler, task, contextAccessor, scopedToken)
                        .ConfigureAwait(false)
                ).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Executes the handler in async scope
        /// </summary>
        private ValueTask ExecuteHandler(IEventHandler<T> handler, Event<T> task, IEventContextAccessor<T> ctx, CancellationToken token)
        {
            ctx.Set(task); // set metadata and begin scope
            using var logScope = _logger.BeginScope(task.Metadata ?? new EventMetadata(Guid.NewGuid().ToString()));

            Task.Run(async () => await handler.Handle(task.Data, token), token).ConfigureAwait(false);

            return ValueTask.CompletedTask;
        }


        private void EnsureStoppingTokenIsCreated()
        {
            if (_stoppingToken is not null && !_stoppingToken.IsCancellationRequested)
            {
                _stoppingToken.Cancel();
            }

            _stoppingToken = new CancellationTokenSource();
        }
        public async ValueTask Stop(CancellationToken token = default)
        {
            await DisposeAsync().ConfigureAwait(false);
        }

    }
}
