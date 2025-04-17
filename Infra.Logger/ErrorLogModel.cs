namespace Infra.Logger
{
    public class ErrorLogModel
    {
        public int Id { get; set; }

        public string ReferenceId { get; set; } = string.Empty;
        public string TraceId { get; set; } = string.Empty;

        public string SourceType { get; set; } = string.Empty;

        public string Operation { get; set; } = string.Empty;

        public string Payload { get; set; } = string.Empty;

        public string Exception { get; set; } = string.Empty;

        public DateTime LoggedAt { get; set; } = DateTime.UtcNow;
    }

}
