using System;
using System.Collections.Generic;
using System.Security.Authentication;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Services;
using BirdyAPI.Tools;
using Microsoft.AspNetCore.Mvc;

namespace BirdyAPI.Controllers
{
    [Route("api/chats")]
    public class ChatsController : Controller
    {
        private readonly ChatsService _chatsService;
        private readonly ToolService _toolService;
        public ChatsController(BirdyContext context)
        {
            _chatsService = new ChatsService(context);
            _toolService = new ToolService(context);
        }

        [HttpGet]
        [Route("getAllChats")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<ChatInfoDto>))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult GetChats([FromQuery] UserSessions currentSession)
        {
            try
            {
                _toolService.ValidateToken(currentSession);
                return Ok(_chatsService.GetAllChats(currentSession.UserId));
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }

        [HttpGet]
        [Route("getChat")]
        [ProducesResponseType(statusCode: 200, type: typeof(ChatInfoDto))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult GetChat([FromQuery] UserSessions currentSession, [FromQuery] Guid chatId)
        {
            try
            {
                _toolService.ValidateToken(currentSession);
                return Ok(_chatsService.GetChat(currentSession.UserId, chatId));
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }

    }
}
