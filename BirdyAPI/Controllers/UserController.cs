using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Security.Authentication;
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
        private readonly ToolService _toolService;

        public UserController(BirdyContext context, IConfiguration configuration)
        {
            _userService = new UserService(context, configuration);
            _toolService = new ToolService(context);
        }
        [HttpGet]
        [Route("get")]
        [ProducesResponseType(statusCode: 200, type: typeof(UserAccountDto))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        public IActionResult GetUserInfo([FromQuery] UserSessions currentSession)
        {
            try
            {
                _toolService.ValidateToken(currentSession);
                return Ok(_userService.SearchUserInfo(currentSession));
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

        [HttpGet]
        [Route("show")]
        [Produces(typeof(List<User>))]
        public IActionResult GetUsers() //Чисто для тестов/проверки/чека бд, так что никакх проверок на токены и т.д.
        {
            try
            {
                return Ok(_userService.GetAllUsers());
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

        [HttpPost]
        [Route("setAvatar")]
        [ProducesResponseType(statusCode: 200, type: typeof(SimpleAnswerDto))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        public IActionResult SetAvatar([FromQuery]UserSessions currentSession, [FromBody] byte[] photoBytes)
        {
            try
            {
                _toolService.ValidateToken(currentSession);
                return Ok(_userService.SetProfileAvatar(currentSession.UserId, photoBytes));
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
