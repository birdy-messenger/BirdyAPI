using BirdyAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BirdyAPI.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("messages")]
    public class MessageController : ExtendedController
    {
        private readonly MessageService _messageService;
        private readonly ToolService _toolService;

        public MessageController(BirdyContext context)
        {
            _messageService = new MessageService(context);
            _toolService = new ToolService(context);
        }
    }
}
