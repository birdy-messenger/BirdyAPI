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
                _friendService.AddFriend();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }

        [HttpGet]
        [Route("getAllFriends")]
        public IActionResult GetFriends()
        {
            try
            {
                _friendService.GetFriends();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }
    }
}
