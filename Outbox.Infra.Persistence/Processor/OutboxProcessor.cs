using Infra.Broker;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Outbox.Infra.Persistence.Processor
{
    public class OutboxProcessor
    {
        private readonly ILogger<OutboxProcessor> _logger;
        private readonly AppDbContext _appDbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IPublisher _publisher;
        public OutboxProcessor(ILogger<OutboxProcessor> logger, IPublishEndpoint publishEndpoint, AppDbContext appDbContext, IPublisher publisher)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _appDbContext = appDbContext;
            _publisher = publisher;
        }

        // Scheduled to run via Hangfire
        public async Task ProcessOutboxAsync()
        {
            var messages = await _appDbContext.OutboxMessages
                .Where(x => x.ProcessedOn == null && x.RetryCount < 5)
                .OrderBy(x => x.OccurredOn)
                .Take(20)
                .ToListAsync();

            foreach (var message in messages)
            {
                try
                {
                    var type = Type.GetType(message.Type);
                    if (type == null) throw new Exception($"Unknown message type: {message.Type}");

                    //throw new Exception("ex");

                    var obj = JsonSerializer.Deserialize(message.Content, type);
                    if (obj == null) throw new Exception("Deserialized object is null");


                    var publishMethod = typeof(IPublisher)
                        .GetMethod("Publish")?
                        .MakeGenericMethod(type);

                    await (Task)publishMethod?.Invoke(_publisher, new[] { obj, message.Id, message.TraceId })!;
                    message.ProcessedOn = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    message.RetryCount++;
                    message.Error = ex.Message;
                    Log
                        .ForContext("ReferenceId", message.Id.ToString())
                        .ForContext("TraceId", (message.TraceId ?? Guid.Empty).ToString())
                        .ForContext("SourceType", "Message")
                        .ForContext("Operation", "KafkaPublish")
                        .ForContext("Payload", JsonSerializer.Serialize(message))
                        .ForContext("LoggedAt", DateTime.UtcNow)
                        .Error(ex, "An error occurred while processing the message.");
                }
            }

            await _appDbContext.SaveChangesAsync();
        }
    }

}
