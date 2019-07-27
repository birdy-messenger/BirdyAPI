using System;
using System.Security.Authentication;
using BirdyAPI.Dto;
using BirdyAPI.Services;
using BirdyAPI.Tools.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BirdyAPI.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("users")]
    public class UserController : ExtendedController
    {
        private readonly UserService _userService;

        public UserController(BirdyContext context)
        {
            _userService = new UserService(context);
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
                int currentUserId = ValidateToken(token);
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
                ValidateToken(token);
                int userId = _userService.GetUserIdByUniqueTag(uniqueTag);
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
    }
}
