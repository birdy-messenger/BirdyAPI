using System;
using BirdyAPI.Services;
using BirdyAPI.Tools.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BirdyAPI.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("messages")]
    public class MessageController : ExtendedController
    {
        private readonly MessageService _messageService;
        private readonly AccessService _accessService;

        public MessageController(BirdyContext context)
        {
            _messageService = new MessageService(context);
            _accessService = new AccessService(context);
        }

        [HttpPost]
        [Route("{uniqueTag}")]
        public IActionResult SendMessageToUser([FromHeader] Guid token, string uniqueTag, [FromBody] string message)
        {
            try
            {
                int currentUserId = _accessService.ValidateToken(token);
                int userId = _toolService.GetUserIdByUniqueTag(uniqueTag);
                _messageService.SendMessageToUser(currentUserId, userId, message);
                
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.SerializeAsResponse());
            }
        }
    }
}
