using System.Text.Json.Serialization;

namespace Messages
{
    public class TestMessage
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = default!;
    }
}
