using System.Threading.Tasks;
using MassTransit;

namespace Core.Infrastructure.RabbitMQ.Messages
{
    public class PublishService : IPublishService
    {
        private readonly IBusControl _busControl;

        public PublishService(IBusControl busControl)
        {
            _busControl = busControl;
        }

        /// <summary>
        /// Pub/Sub approach.
        /// For each message published, a copy of the message is delivered to each subscriber.
        /// https://masstransit-project.com/usage/producers.html#publish
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public async ValueTask PublishAsync(object @event)
        {
            await _busControl.Publish(@event);
        }
    }
}
