using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    [Route("chats")]
    public class ChatController : ExtendedController
    {
        private readonly ChatService _chatService;
        private readonly ToolService _toolService;
        private readonly AccessService _accessService;
        public ChatController(BirdyContext context)
        {
            _chatService = new ChatService(context);
            _toolService = new ToolService(context);
            _accessService = new AccessService(context);
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
                return Ok(_chatService.GetChats(currentUserId));
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
                return Ok(_chatService.GetChatInfo(currentUserId, chatId));
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

        /// <summary>
        /// Create chat with user friends
        /// </summary>
        /// <response code = "200">Chat created</response>
        /// <response code = "500">Unexpected Exception (only for debug)</response>
        /// <response code = "401">Invalid token</response>
        /// <response code = "404">User by tag not found</response>;
        [HttpPost]
        [ProducesResponseType(statusCode: 200, type: typeof(List<ChatInfoDto>))]
        [ProducesResponseType(statusCode: 500, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        [ProducesResponseType(statusCode: 404, type: typeof(void))]

        public IActionResult CreateChat([FromHeader] Guid token, [FromBody] string[] friendsUniqueTags)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                List<int> friendsId = friendsUniqueTags.Select(k => _toolService.GetUserIdByUniqueTag(k)).ToList();
                friendsId.RemoveAll(k => !_toolService.IsItUserFriend(currentUserId ,k));
                _chatService.CreateChat(friendsId, currentUserId);
                return Ok();
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch(ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.SerializeAsResponse());
            }
        }

        /// <summary>
        /// Add friend to chat
        /// </summary>
        /// <response code = "200">Friend added</response>
        /// <response code = "500">Unexpected Exception (only for debug)</response>
        /// <response code = "401">Invalid token</response>
        /// <response code = "403">User has no rights for this action</response>
        /// <response code = "404">User by tag not found</response>;
        [HttpPatch]
        [Route("{chatNumber}")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<ChatInfoDto>))]
        [ProducesResponseType(statusCode: 500, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        [ProducesResponseType(statusCode: 403, type: typeof(void))]
        [ProducesResponseType(statusCode: 404, type: typeof(void))]

        public IActionResult AddFriendToChat([FromHeader] Guid token, [FromBody] string friendUniqueTag, int chatNumber)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                int friendId = _toolService.GetUserIdByUniqueTag(friendUniqueTag);
                _accessService.CheckChatUserAccess(currentUserId, chatNumber, ChatStatus.User);
                if (!_toolService.IsItUserFriend(currentUserId, friendId))
                    throw new DataException();
                _chatService.AddUserToChat(currentUserId, friendId, chatNumber);
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
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.SerializeAsResponse());
            }
        }
    }
}
