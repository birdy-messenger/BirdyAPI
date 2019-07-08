using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Services;
using BirdyAPI.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BirdyAPI.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(BirdyContext context, IConfiguration configuration)
        {
            _userService = new UserService(context, configuration);
        }
        [HttpGet]
        [Route("get")]
        [ProducesResponseType(statusCode: 200, type: typeof(UserAccountDto))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
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
        public IActionResult GetUsers()
        {
            try
            {
                return Ok(_userService.GetAllUsers());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }

        [HttpPost]
        [Route("setAvatar")]
        [ProducesResponseType(statusCode: 200, type: typeof(SimpleAnswerDto))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        public IActionResult SetAvatar([FromQuery]int id, [FromBody] byte[] photoBytes)
        {
            try
            {
                return Ok(_userService.SetProfileAvatar(id, photoBytes));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }
    }
}
