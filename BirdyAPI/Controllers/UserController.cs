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
    [Route("user")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly ToolService _toolService;

        public UserController(BirdyContext context, IConfiguration configuration)
        {
            _userService = new UserService(context, configuration);
            _toolService = new ToolService(context);
        }


        /// <summary>
        /// Current user data
        /// </summary>
        /// <response code = "200">Return current user data in JSON</response>
        /// <response code = "400">Exception message</response>
        /// <response code = "401">Invalid token</response>
        [HttpGet]
        [ProducesResponseType(statusCode: 200, type: typeof(UserAccountDto))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult GetMySelfInfo([FromHeader] Guid token)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                return Ok(_userService.SearchMySelfInfo(currentUserId));
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
        /// User data
        /// </summary>
        /// <response code = "200">Return user data in JSON</response>
        /// <response code = "400">Exception message</response>
        /// <response code = "401">Invalid token</response>
        [HttpGet]
        [Route("{uniqueTag}")]
        [ProducesResponseType(statusCode: 200, type: typeof(UserAccountDto))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult GetUserInfo([FromHeader] Guid token, string uniqueTag)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                return Ok(_userService.SearchUserInfo(uniqueTag));
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
    }
}
