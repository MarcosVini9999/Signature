namespace Signature.API.Domain.Interfaces
{
    public interface IMessageBroker 
    { 
        Task PublishAsync(string topic, object message); 
    }
}
