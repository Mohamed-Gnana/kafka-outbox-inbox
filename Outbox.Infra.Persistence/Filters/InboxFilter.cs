using MassTransit;
using Microsoft.EntityFrameworkCore;
using Outbox.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outbox.Infra.Persistence.Filters
{
    public class InboxFilter<T> : IFilter<ConsumeContext<T>> where T : class
    {
        private readonly AppDbContext _db;

        public InboxFilter(AppDbContext db)
        {
            _db = db;
        }

        public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
        {
            if (!context.MessageId.HasValue)
            {
                // No MessageId — can't dedupe
                await next.Send(context);
                return;
            }

            var messageId = context.MessageId.Value;
            var traceId = context.ConversationId;
            var consumerType = typeof(T).FullName!;

            var alreadyProcessed = await _db.InboxMessages
                .AnyAsync(x => x.Id == messageId && x.ConsumerType == consumerType);

            if (alreadyProcessed)
            {
                return;
            }

            await next.Send(context);

            _db.InboxMessages.Add(new InboxMessage
            {
                Id = messageId,
                TraceId = traceId,
                ConsumerType = consumerType,
                ReceivedOn = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
        }

        public void Probe(ProbeContext context) => context.CreateFilterScope("inboxFilter");
    }
}
