namespace Outbox.Domain
{
    public class InboxMessage
    {
        public Guid Id { get; set; }
        public Guid? TraceId { get; set; }
        public DateTime ReceivedOn { get; set; }
        public string ConsumerType { get; set; } = null!;
    }
}
