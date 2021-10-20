using System.Threading.Tasks;

namespace Core.Infrastructure.RabbitMQ.Messages
{
    public interface IPublishService
    {
        ValueTask PublishAsync(object @event);
    }
}
