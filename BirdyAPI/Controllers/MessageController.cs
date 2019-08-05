using System;
using System.Security.Authentication;
using BirdyAPI.Dto;
using BirdyAPI.Services;
using BirdyAPI.Tools.Exceptions;
using BirdyAPI.Tools.Extensions;
using BirdyAPI.Types;
using Microsoft.AspNetCore.Mvc;

namespace BirdyAPI.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("messages")]
    public class MessageController : ExtendedController
    {
        private readonly MessageService _messageService;
        private readonly UserService _userService;
        private readonly ChatService _chatService;

        public MessageController(BirdyContext context)
        {
            _messageService = new MessageService(context);
            _userService = new UserService(context);
            _chatService = new ChatService(context);
        }


        /// <summary>
        /// Send message to user
        /// </summary>
        /// <response code = "200">Message sent</response>
        /// <response code = "500">Unexpected Exception (only for debug)</response>
        /// <response code = "401">Invalid token</response>
        /// <response code = "404">User not found</response>
        [HttpPost]
        [Route("{uniqueTag}")]
        [ProducesResponseType(statusCode: 200, type: typeof(void))]
        [ProducesResponseType(statusCode: 500, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        [ProducesResponseType(statusCode: 404, type: typeof(void))]
        public IActionResult SendMessageToUser([FromHeader] Guid token, string uniqueTag, [FromBody] string message)
        {
            try
            {
                int currentUserId = ValidateToken(token);
                int userId = _userService.GetUserIdByUniqueTag(uniqueTag);
                _messageService.SendMessageToUser(currentUserId, userId, message);
                return Ok();
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Send message to chat
        /// </summary>
        /// <response code = "200">Message sent</response>
        /// <response code = "500">Unexpected Exception (only for debug)</response>
        /// <response code = "401">Invalid token</response>
        [HttpPost]
        [Route("{chatNumber}")]
        [ProducesResponseType(statusCode: 200, type: typeof(void))]
        [ProducesResponseType(statusCode: 500, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult SendMessageToChat([FromHeader] Guid token, int chatNumber, [FromBody] string message)
        {
            try
            {
                int currentUserId = ValidateToken(token);
                Guid currentChatId = _chatService.GetChatIdByChatNumberAndUserId(currentUserId, chatNumber);
                CheckChatAccess(currentChatId, currentUserId, ChatStatus.User);
                _messageService.SendMessageToChat(currentUserId, currentChatId, message);
                return Ok();
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (InsufficientRightsException)
            {
                return Forbid();
            }
        }
    }
}
