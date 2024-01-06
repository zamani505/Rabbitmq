using System.Collections.Immutable;
using System.Drawing;
using System.Text;
using Newtonsoft.Json;
using RabbitSample.Contracts;

namespace RabbitSample.Services
{
    public class MessageProducer: IMessageProducer
    {
        public MessageProducer()
        {
            
        }
        public void SendMessage<T>(T message, string queueName, IConnectionService connectionService)
        {
            using (var channel = connectionService.Connect("172.16.21.31", "15672"))
            {
                    if (channel != null)
                    {
                    channel.QueueDeclare(queueName, false, false, false, ImmutableDictionary<string, object>.Empty);
                    var json = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(json);
                    channel.BasicPublish(exchange: "", routingKey: queueName, mandatory: false,
                        basicProperties: null, body: body);

                }
            }


        }
    }
}
