using Outbox.Domain;
using Outbox.Domain.Interfaces;
using System.Text.Json;

namespace Outbox.Infra.Persistence.Interfaces
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly AppDbContext _dbContext;

        public MessagePublisher(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task PublishAsync<T>(T @event, Guid? traceId = null)
        {
            var outbox = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                TraceId = traceId,
                OccurredOn = DateTime.UtcNow,
                Type = typeof(T).AssemblyQualifiedName!,
                Content = JsonSerializer.Serialize(@event),
                RetryCount = 0
            };

            _dbContext.OutboxMessages.Add(outbox);
            await _dbContext.SaveChangesAsync();
        }
    }
}
