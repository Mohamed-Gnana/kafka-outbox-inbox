using MassTransit;
using Messages;
using Microsoft.Extensions.Caching.Memory;

namespace Consumer.Consumers
{
    public class TestMessageConsumer : IConsumer<TestMessage>
    {
        private readonly IMemoryCache _memoryCache;

        public TestMessageConsumer(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task Consume(ConsumeContext<TestMessage> context)
        {
            if(_memoryCache.TryGetValue("messages", out object? value) && value is List<TestMessage> messages)
            {
                messages.Add(context.Message);
                _memoryCache.Set("messages", messages);
            }
            else
            {
                _memoryCache.Set("messages", new List<TestMessage>() { context.Message });
            }

            return Task.CompletedTask;
        }
    }
}
