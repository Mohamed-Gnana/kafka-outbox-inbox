using MassTransit;
using Messages;
using Infra.Broker.Kafka;
using Microsoft.AspNetCore.Mvc;
using Outbox.Domain.Interfaces;

namespace Producer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProducersController : ControllerBase
    {
        private readonly IBusControl _busControl;
        private readonly ITopicProducerProvider _topicProducerProvider;
        private readonly IMessagePublisher _messagePublisher;

        private readonly ILogger<ProducersController> _logger;

        public ProducersController(ILogger<ProducersController> logger, IBusControl busControl = null, ITopicProducerProvider topicProducerProvider = null, IMessagePublisher messagePublisher = null)
        {
            _logger = logger;
            _busControl = busControl;
            _topicProducerProvider = topicProducerProvider;
            _messagePublisher = messagePublisher;
        }

        [HttpPost("send")]
        public async Task<ActionResult> Send(TestMessage message)
        {
            // Trace Id should be captured from the header
            await _messagePublisher.PublishAsync(message, message.Id);

            return Ok();
        }
    }
}
