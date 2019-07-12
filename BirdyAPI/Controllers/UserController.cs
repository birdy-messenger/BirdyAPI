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
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("users")]
    public class UserController : ExtendedController
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
        /// <response code = "500">Unexpected Exception (only for debug)</response>
        /// <response code = "401">Invalid token</response>
        [HttpGet]
        [ProducesResponseType(statusCode: 200, type: typeof(UserAccountDto))]
        [ProducesResponseType(statusCode: 500, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult GetMySelfInfo([FromHeader] Guid token)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                return Ok(_userService.GetUserInfo(currentUserId));
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.SerializeAsResponse());
            }
        }

        /// <summary>
        /// User data
        /// </summary>
        /// <response code = "200">Return user data in JSON</response>
        /// <response code = "500">Unexpected Exception (only for debug)</response>
        /// <response code = "401">Invalid token</response>
        /// <response code = "404">User by tag not found</response>
        [HttpGet]
        [Route("{uniqueTag}")]
        [ProducesResponseType(statusCode: 200, type: typeof(UserAccountDto))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        [ProducesResponseType(statusCode: 404, type: typeof(void))]
        public IActionResult GetUserInfo([FromHeader] Guid token, string uniqueTag)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                int userId = _toolService.GetUserIdByUniqueTag(uniqueTag);
                return Ok(_userService.GetUserInfo(userId));
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.SerializeAsResponse());
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
