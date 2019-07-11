using System;
using System.Collections.Generic;
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
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult GetUserInfo([FromQuery] Guid token)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                return Ok(_userService.SearchUserInfo(currentUserId));
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
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult SetAvatar([FromQuery] Guid token, [FromBody] byte[] photoBytes)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                return Ok(_userService.SetProfileAvatar(currentUserId, photoBytes));
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
