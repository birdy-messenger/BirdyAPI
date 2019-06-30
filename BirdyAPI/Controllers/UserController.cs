using System;
using System.Collections.Generic;
using BirdyAPI.DataBaseModels;
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
        [HttpGet]
        [Route("get")]
        [Produces(typeof(UserAccountDto))]
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

        [HttpGet]
        [Route("show")]
        [Produces(typeof(List<User>))]
        public IEnumerable<User> GetUsers()
        {
            return _userService.GetAllUsers();
        }
    }
}
