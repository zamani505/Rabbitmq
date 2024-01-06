using System.Collections.Immutable;
using System.Drawing;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RabbitSample.Contracts;
using RabbitSample.Event.EventBus;
using RabbitSample.Models;
using IBasicConsumer = RabbitMQ.Client.IBasicConsumer;

namespace RabbitSample.Services
{
    public class MessageConsumer : IMessageConsumer
    {
        public static EventingBasicConsumer consumer { get; set; }
        private readonly IConnectionFactory _connectionFactory;
        EventBusRabbitMQ _eventBusRabbitMQ;
        IConnection _connection;
        bool _disposed;
        public MessageConsumer(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            if (!IsConnected)
            {
                TryConnect();
            }
        }

        public bool IsConnected
        {
            get
            {
                return _connection == null ? _disposed : _connection.IsOpen;
            }
        }
        public bool TryConnect()
        {
            try
            {
                _connection = _connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException e)
            {
                Thread.Sleep(5000);
                _connection = _connectionFactory.CreateConnection();
            }

            if (IsConnected)
            {
                _connection.ConnectionShutdown += OnConnectionShutdown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;
                return true;
            }

            return false;
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                if(!TryConnect())
                    throw new InvalidOperationException("No RabbitMQ connections are available ");
            }

            return _connection.CreateModel();
        }

        public void CreateConsumerChannel()
        {
            _eventBusRabbitMQ = new EventBusRabbitMQ(this, "insertOrder");
            _eventBusRabbitMQ.CreateConsumerChannel();
        }

        public void Disconnect()
        {
            if (_disposed)
            {
                return;
            }
            Dispose();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            try
            {
                _connection.Dispose();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;
            TryConnect();
        }

        void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;
            TryConnect();
        }

        void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;
            TryConnect();
        }
    }
}
