using System;
using BirdyAPI.Dto;
using BirdyAPI.Services;
using BirdyAPI.Tools;
using Microsoft.AspNetCore.Mvc;

namespace BirdyAPI.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserContext context)
        {
            _userService = new UserService(context);
        }
        public IActionResult GetUserInfo([FromQuery] UserSessionDto user)
        {
            try
            {
                return Ok(_userService.SearchUserInfo(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }
    }
}
