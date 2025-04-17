namespace Outbox.Domain
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public Guid? TraceId { get; set; }
        public DateTime OccurredOn { get; set; }
        public string Type { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime? ProcessedOn { get; set; }
        public int RetryCount { get; set; }
        public string? Error { get; set; }
    }
}
