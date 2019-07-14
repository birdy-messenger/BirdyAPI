using System;
using System.Collections.Generic;
using System.Security.Authentication;
using BirdyAPI.Dto;
using BirdyAPI.Services;
using BirdyAPI.Tools;
using Microsoft.AspNetCore.Mvc;

namespace BirdyAPI.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("chats")]
    public class ChatsController : ExtendedController
    {
        private readonly ChatsService _chatsService;
        private readonly ToolService _toolService;
        public ChatsController(BirdyContext context)
        {
            _chatsService = new ChatsService(context);
            _toolService = new ToolService(context);
        }

        /// <summary>
        /// Get all user chats
        /// </summary>
        /// <response code = "200">Return chats info</response>
        /// <response code = "500">Unexpected Exception (only for debug)</response>
        /// <response code = "401">Invalid token</response>
        [HttpGet]
        [ProducesResponseType(statusCode: 200, type: typeof(List<ChatInfoDto>))]
        [ProducesResponseType(statusCode: 500, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult GetChats([FromHeader] Guid token)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                return Ok(_chatsService.GetChats(currentUserId));
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.SerializeAsResponse());
            }
        }

        /// <summary>
        /// Get chat info
        /// </summary>
        /// <response code = "200">Return chat info</response>
        /// <response code = "500">Unexpected Exception (only for debug)</response>
        /// <response code = "403">User isn't in this chat</response>
        /// <response code = "401">Invalid token</response>
        [HttpGet]
        [Route("{chatId}")]
        [ProducesResponseType(statusCode: 200, type: typeof(ChatInfoDto))]
        [ProducesResponseType(statusCode: 403, type: typeof(void))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        [ProducesResponseType(statusCode: 500, type: typeof(ExceptionDto))]
        public IActionResult GetChat([FromHeader] Guid token, Guid chatId)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                return Ok(_chatsService.GetChatInfo(currentUserId, chatId));
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (ArgumentException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.SerializeAsResponse());
            }
        }

    }
}
