
using Confluent.Kafka;
using MassTransit;

namespace Infra.Broker.Kafka.Interfaces
{
    public class Publisher : IPublisher
    {
        private readonly IBusControl _busControl;
        private readonly ITopicProducerProvider _topicProducerProvider;

        public Publisher(ITopicProducerProvider topicProducerProvider, IBusControl busControl)
        {
            _topicProducerProvider = topicProducerProvider;
            _busControl = busControl;
        }

        public async Task Publish<T>(T message, Guid? messageId = null, Guid? traceId = null) where T : class
        {
            await _busControl.StartAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);
            try
            {
                var producer = _topicProducerProvider.GetProducer<T>(new Uri($"topic:{typeof(T).GetMessageTopic()}"));
                var pipe = Pipe.Execute<KafkaSendContext<T>>(context =>
                {
                    context.MessageId = messageId ?? Guid.NewGuid();
                    context.ConversationId = traceId ?? null;
                });
                await producer.Produce(message, pipe);
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                await _busControl.StopAsync();
            }
        }
    }
}
