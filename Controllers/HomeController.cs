using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitSample.Contracts;
using RabbitSample.Models;

namespace RabbitSample.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly IMessageProducer _messageProducer;
        private readonly IMessageConsumer _consumer;
        private readonly IConnectionService _connectionService;

        public HomeController(IMessageProducer messageProducer,IMessageConsumer consumer,IConnectionService connectionService)
        {
            _messageProducer = messageProducer;
            _consumer = consumer;
            _connectionService = connectionService;
           
        }
        public ActionResult Index()
        {
            return View();
        }
     
        [HttpGet("SendMessage")]
        public ActionResult SendMessage()
        {
            var order = new Order() {Price = 500,ProductName = "Cado",Quantity = 1};
            _messageProducer.SendMessage(message:order,queueName:"test",_connectionService);
            return View();
        }


        [HttpGet("RecMessage")]
        public ActionResult RecMessage()
        {
           
        //  _consumer.ReceiveMessage("test",_connectionService);
          
            return View();
        }

    }
}
