namespace Outbox.Domain.Interfaces
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(T @event, Guid? requestId = null);
    }
}
