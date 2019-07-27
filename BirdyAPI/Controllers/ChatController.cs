using System;
using System.Collections.Generic;
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
        private readonly FriendService _friendService;
        private readonly UserService _userService;
        public ChatController(BirdyContext context) : base(context)
        {
            _chatService = new ChatService(context);
            _friendService = new FriendService(context);
            _userService = new UserService(context);
        }

        /// <summary>
        /// Get all user chats
        /// </summary>
        /// <response code = "200">Return chats info</response>
        /// <response code = "500">Unexpected Exception (only for debug)</response>
        /// <response code = "401">Invalid token</response>
        [HttpGet]
        [ProducesResponseType(statusCode: 200, type: typeof(List<ChatPreviewDto>))]
        [ProducesResponseType(statusCode: 500, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult GetChats([FromHeader] Guid token)
        {
            try
            {
                int currentUserId = ValidateToken(token);
                return Ok(_chatService.GetChatsPreview(currentUserId));
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
        [Route("{chatNumber}")]
        [ProducesResponseType(statusCode: 200, type: typeof(ChatInfoDto))]
        [ProducesResponseType(statusCode: 403, type: typeof(void))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        [ProducesResponseType(statusCode: 500, type: typeof(ExceptionDto))]
        public IActionResult GetChat([FromHeader] Guid token, int chatNumber)
        {
            try
            {
                int currentUserId = ValidateToken(token);
                CheckChatAccess(currentUserId, chatNumber, ChatStatus.User);
                return Ok(_chatService.GetChat(currentUserId, chatNumber));
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (InsufficientRightsException)
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
        [ProducesResponseType(statusCode: 200, type: typeof(List<ChatPreviewDto>))]
        [ProducesResponseType(statusCode: 500, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        [ProducesResponseType(statusCode: 404, type: typeof(void))]

        public IActionResult CreateChat([FromHeader] Guid token, [FromBody] string[] friendsUniqueTags)
        {
            try
            {
                int currentUserId = ValidateToken(token);
                List<int> friendsId = friendsUniqueTags.Select(k => _userService.GetUserIdByUniqueTag(k)).ToList();
                friendsId.RemoveAll(k => !_friendService.IsItUserFriend(currentUserId ,k));
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
        [ProducesResponseType(statusCode: 200, type: typeof(List<ChatPreviewDto>))]
        [ProducesResponseType(statusCode: 500, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        [ProducesResponseType(statusCode: 403, type: typeof(void))]
        [ProducesResponseType(statusCode: 404, type: typeof(void))]

        public IActionResult AddFriendToChat([FromHeader] Guid token, [FromBody] string friendUniqueTag, int chatNumber)
        {
            try
            {
                int currentUserId = ValidateToken(token);
                int friendId = _userService.GetUserIdByUniqueTag(friendUniqueTag);
                CheckChatAccess(currentUserId, chatNumber, ChatStatus.User);
                if (!_friendService.IsItUserFriend(currentUserId, friendId))
                    throw new InsufficientRightsException();
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

        /// <summary>
        /// Rename chat
        /// </summary>
        /// <response code = "200">Chat renamed</response>
        /// <response code = "500">Unexpected Exception (only for debug)</response>
        /// <response code = "401">Invalid token</response>
        /// <response code = "403">User has no rights for this action</response>
        [HttpPut]
        [Route("{chatNumber}")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<ChatPreviewDto>))]
        [ProducesResponseType(statusCode: 500, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        [ProducesResponseType(statusCode: 403, type: typeof(void))]

        public IActionResult RenameChat([FromHeader] Guid token, int chatNumber, [FromBody] string newChatName)
        {
            try
            {
                int currentUserId = ValidateToken(token);
                CheckChatAccess(currentUserId, chatNumber, ChatStatus.User);
                _chatService.RenameChat(currentUserId, chatNumber, newChatName);
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
            catch (Exception ex)
            {
                return InternalServerError(ex.SerializeAsResponse());
            }
        }

        /// <summary>
        /// Kick user from chat
        /// </summary>
        /// <response code = "200">User kicked</response>
        /// <response code = "500">Unexpected Exception (only for debug)</response>
        /// <response code = "401">Invalid token</response>
        /// <response code = "403">User has no rights for this action</response>
        /// <response code = "404">User for kick not found</response>
        [HttpDelete]
        [Route("{chatNumber}")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<ChatPreviewDto>))]
        [ProducesResponseType(statusCode: 500, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        [ProducesResponseType(statusCode: 403, type: typeof(void))]
        [ProducesResponseType(statusCode: 404, type: typeof(void))]

        public IActionResult KickUserFromChat([FromHeader] Guid token, int chatNumber, [FromBody] string userUniqueTag)
        {
            try
            {
                int currentUserId = ValidateToken(token);
                CheckChatAccess(currentUserId, chatNumber, ChatStatus.Admin);
                int userId = _userService.GetUserIdByUniqueTag(userUniqueTag);
                _chatService.KickUser(currentUserId, chatNumber, userId);
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
            catch (DataNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.SerializeAsResponse());
            }
        }

        /// <summary>
        /// Leave from chat
        /// </summary>
        /// <response code = "200">User kicked</response>
        /// <response code = "500">Unexpected Exception (only for debug)</response>
        /// <response code = "401">Invalid token</response>
        /// <response code = "403">User has no rights for this action</response>
        [HttpDelete]
        [Route("leave/{chatNumber}")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<ChatPreviewDto>))]
        [ProducesResponseType(statusCode: 500, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        [ProducesResponseType(statusCode: 403, type: typeof(void))]

        public IActionResult LeaveFromChat([FromHeader] Guid token, int chatNumber)
        {
            try
            {
                int currentUserId = ValidateToken(token);
                CheckChatAccess(currentUserId, chatNumber, ChatStatus.User);
                _chatService.LeaveFromChat(currentUserId, chatNumber);
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
            catch (DataNotFoundException)
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
