using System;
using System.Threading.Tasks;
using Core.Infrastructure.RabbitMQ.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using Shared;

namespace Producer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly IPublishService _publishService;

        public MessageController(ILogger<MessageController> logger, IPublishService publishService)
        {
            _logger = logger;
            _publishService = publishService;
        }

        [HttpPost]
        public async ValueTask<IActionResult> GenerateMessage()
        {
            Message message = new Message(Guid.NewGuid(), $"Body {Guid.NewGuid()}");

            _logger.LogInformation($"Producer: {message.Id}");
            
            await _publishService.PublishAsync(message);
            return Ok(message);
        }
    }
}
