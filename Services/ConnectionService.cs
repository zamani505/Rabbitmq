using System.Net;
using RabbitMQ.Client;
using RabbitSample.Contracts;

namespace RabbitSample.Services
{
    public class ConnectionService : IConnectionService
    {
        public IModel? Connect(string hostName, string port)
        {

            var factory = new ConnectionFactory
            {
                // HostName = hostName, Port = int.Parse(port),  UserName = "guest", Password = "guest" ,
                //Ssl = new SslOption{Enabled = false},
                Uri = new Uri($"amqp:/guest:guest@{hostName}:{port}"), 
                VirtualHost = "/",
               // Endpoint = new AmqpTcpEndpoint(hostName, int.Parse(port)),
                //ContinuationTimeout = TimeSpan.Parse("00:00:10"),
                UserName = "guest",
                Password = "guest"


            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            return channel;
        }
    }
}
