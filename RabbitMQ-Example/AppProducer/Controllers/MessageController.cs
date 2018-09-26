using Models;
using RabbitMQLayer;
using System.Web.Mvc;
using Global;

namespace AppProducer.Controllers
{
    public class MessageController : Controller
    {
        // GET: Message
        public ActionResult Send()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Send(Message msg)
        {
            Factory.Publish(Settings.GetFactorySettings(), "TEST", msg);
            return View();
        }
    }
}