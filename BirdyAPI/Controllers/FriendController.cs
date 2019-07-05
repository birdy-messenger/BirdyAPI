using System;
using BirdyAPI.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BirdyAPI.Controllers
{
    [Route("api/friends")]
    public class FriendController : Controller
    {
        [HttpPost]
        [Route("addFriend")]
        public IActionResult AddFriend([FromQuery] FriendRequestDto friendRequest)
        {
            throw new NotImplementedException();
        }
    }
}
