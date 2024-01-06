using RabbitMQ.Client;
using RabbitSample.Models;

namespace RabbitSample.Contracts
{
    public interface IMessageConsumer : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();
        IModel CreateModel();
        void CreateConsumerChannel();

        void Disconnect();
        
    }
}
