using System;
using System.Data;
using System.Security.Authentication;
using BirdyAPI.Dto;
using BirdyAPI.Services;
using BirdyAPI.Tools;
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

        [HttpPost]
        [Route("{uniqueTag}")]
        public IActionResult SendMessageToUser([FromHeader] Guid token, string uniqueTag, [FromBody] string message)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
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
