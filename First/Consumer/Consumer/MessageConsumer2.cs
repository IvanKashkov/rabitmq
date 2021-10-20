using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared;

namespace Consumer.Consumer
{
    public class Message2Consumer : IConsumer<Message2>
    {
        private readonly ILogger<Message2Consumer> _logger;

        public Message2Consumer(ILogger<Message2Consumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<Message2> context)
        {
            _logger.LogInformation($"Consumer: {context.Message.Id}");
            return Task.CompletedTask;
        }
    }
}
