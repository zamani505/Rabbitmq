using System.Text;
using Newtonsoft.Json;
using QueueManager.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitSample.Contracts;
using RabbitSample.Models;

namespace RabbitSample.Event.EventBus
{
    public class EventBusRabbitMQ:IDisposable
    {
        private readonly IMessageConsumer _messageConsumer;
        private string _queueName;
        private IModel _consumerChannel;

        public EventBusRabbitMQ(IMessageConsumer messageConsumer,string queueName=null)
        {
            _messageConsumer = messageConsumer;
            _queueName = queueName;
        }
        public IModel CreateConsumerChannel()
        {
            if (!_messageConsumer.IsConnected)
            {
                _messageConsumer.TryConnect();
            }

            var channel = _messageConsumer.CreateModel();
            channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

          
            consumer.Received += ReceivedEvent;



            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
            };
            return channel;
        }
        private void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey == "insertOrder")
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                var order = JsonConvert.DeserializeObject<Order>(message);
               
            }

         
        }
        public void Dispose()
        {
            _consumerChannel.Dispose();
        }
    }
}
