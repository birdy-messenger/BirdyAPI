using System;
using BirdyAPI.Dto;
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
        public IActionResult AddFriend([FromQuery] FriendRequestDto friendRequest)
        {
            try
            {
                _friendService.AddFriend(friendRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }

        [HttpGet]
        [Route("getAllFriends")]
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
    }
}
