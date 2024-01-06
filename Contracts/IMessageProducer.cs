namespace RabbitSample.Contracts
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message,string queueName, IConnectionService connectionService);
    }
}
