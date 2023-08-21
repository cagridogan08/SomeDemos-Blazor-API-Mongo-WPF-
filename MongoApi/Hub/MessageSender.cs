using Microsoft.AspNetCore.SignalR;

namespace MongoApi.Hub
{
    public class MessageSender : BackgroundService
    {
        private readonly MessageHub _messageHub;

        public MessageSender(MessageHub messageHub)
        {
            _messageHub = messageHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    _messageHub.Clients.All.SendAsync("Send", $"{DateTime.Now}", cancellationToken: stoppingToken);
                }
            }, stoppingToken);
        }
    }
}
