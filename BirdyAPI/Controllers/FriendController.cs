using System;
using System.Collections.Generic;
using System.Security.Authentication;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Models;
using BirdyAPI.Services;
using BirdyAPI.Tools;
using Microsoft.AspNetCore.Mvc;

namespace BirdyAPI.Controllers
{
    [Route("friends")]
    public class FriendController : Controller
    {
        private readonly FriendService _friendService;
        private readonly ToolService _toolService;

        public FriendController(BirdyContext context)
        {
            _friendService = new FriendService(context);
            _toolService = new ToolService(context);
        }


        /// <summary>
        /// Send friend request
        /// </summary>
        /// <response code = "200">Request sent</response>
        /// <response code = "400">Exception message</response>
        /// <response code = "401">Invalid token</response>
        [HttpPost]
        [ProducesResponseType(statusCode: 200, type: typeof(void))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult SendFriendRequest([FromBody] string userUniqueTag, [FromHeader] Guid token)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                _friendService.SendFriendRequest(userUniqueTag, currentUserId);
                return Ok();
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

        /// <summary>
        /// Accept friend request
        /// </summary>
        /// <response code = "200">Request accepted</response>
        /// <response code = "400">Exception message</response>
        /// <response code = "401">Invalid token</response>
        [HttpPatch]
        [ProducesResponseType(statusCode: 200, type: typeof(void))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult AcceptFriendRequest([FromBody] string userUniqueTag, [FromHeader] Guid token)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                _friendService.AcceptFriendRequest(userUniqueTag, currentUserId);
                return Ok();
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

        /// <summary>
        /// Get friends
        /// </summary>
        /// <response code = "200">Return list of friends</response>
        /// <response code = "400">Exception message</response>
        /// <response code = "401">Invalid token</response>
        [HttpGet]
        [ProducesResponseType(statusCode: 200, type: typeof(List<UserFriend>))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult GetFriends([FromHeader] Guid token)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                return Ok(_friendService.GetFriends(currentUserId));
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


        /// <summary>
        /// Delete user for friend
        /// </summary>
        /// <response code = "200">Friend deleted</response>
        /// <response code = "400">Exception message</response>
        /// <response code = "401">Invalid token</response>
        [HttpDelete]
        [Route("{friendUniqueTag}")]
        [ProducesResponseType(statusCode: 200, type: typeof(void))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult DeleteFriend(string friendUniqueTag, [FromHeader] Guid token)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                _friendService.DeleteFriend(currentUserId, friendUniqueTag);
                return Ok();
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
