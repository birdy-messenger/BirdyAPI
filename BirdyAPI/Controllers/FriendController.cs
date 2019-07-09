using System;
using System.Collections.Generic;
using BirdyAPI.Dto;
using BirdyAPI.Models;
using BirdyAPI.Services;
using BirdyAPI.Tools;
using Microsoft.AspNetCore.Mvc;

namespace BirdyAPI.Controllers
{
    [Route("api/friends")]
    public class FriendController : Controller
    {
        private readonly FriendService _friendService;

        public FriendController(BirdyContext context)
        {
            _friendService = new FriendService(context);
        }

        [HttpPost]
        [Route("addFriend")]
        [ProducesResponseType(statusCode: 200, type: typeof(FriendRequestAnswerDto))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        public IActionResult AddFriend([FromQuery] FriendRequestDto friendRequest)
        {
            try
            {
                return Ok(_friendService.AddFriend(friendRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }

        [HttpGet]
        [Route("allFriends")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<UserFriend>))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        public IActionResult GetFriends([FromQuery] int userId)
        {
            try
            {
                return Ok(_friendService.GetFriends(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }

        [HttpDelete]
        [Route("deleteFriend")]
        [ProducesResponseType(statusCode: 200, type: typeof(SimpleAnswerDto))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        public IActionResult DeleteFriend([FromQuery] int userId, int friendId)
        {
            try
            {
                return Ok(_friendService.DeleteFriend(userId, friendId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }
    }
}
