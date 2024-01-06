using RabbitMQ.Client;

namespace RabbitSample.Contracts
{
    public interface IConnectionService
    {
        IModel? Connect(string hostName,string port);
    }
}
