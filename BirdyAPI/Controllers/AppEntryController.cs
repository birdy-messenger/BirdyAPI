using System;
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
    [Route("app")]
    public class AppEntryController : Controller
    {
        private readonly AppEntryService _appEntryService;
        private readonly ToolService _toolService;

        public AppEntryController(BirdyContext context, IConfiguration configuration)
        {
            _appEntryService = new AppEntryService(context, configuration);
            _toolService = new ToolService(context);
        }

        /// <summary>
        /// User authentication
        /// </summary>
        /// <response code = "200">Return user token</response>
        /// <response code = "400">Exception message</response>
        /// <response code = "401">User need to confirm email</response>
        [HttpPost]
        [Route("authentication")]
        [ProducesResponseType(statusCode: 200, type:typeof(SimpleAnswerDto))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type:typeof(void))]
        public IActionResult UserAuthentication([FromBody] AuthenticationDto user)
        {
            try
            {
                return Ok(_appEntryService.Authentication(user));
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
        /// User registration
        /// </summary>
        /// <response code = "200">Confirm message sent</response>
        /// <response code = "400">Exception message</response>
        /// <response code = "409">Duplicate account</response>
        [HttpPost]
        [Route("registration")]
        [ProducesResponseType(statusCode: 200, type: typeof(void))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 409, type: typeof(void))]
        public IActionResult UserRegistration([FromBody]RegistrationDto user)
        {
            try
            {
                _appEntryService.CreateNewAccount(user);
                return Ok();
            }
            catch(DuplicateAccountException)
            {
                return Conflict();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("confirm")]
        public IActionResult EmailConfirming([FromQuery] int id)
        {
            try
            {
                return Ok(_appEntryService.GetUserConfirmed(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }

        /// <summary>
        /// Change user password
        /// </summary>
        /// <response code = "200">Password changed</response>
        /// <response code = "400">Exception message</response>
        /// <response code = "401">Invalid token</response>
        [HttpPut]
        [Route("password/{token}")]
        [ProducesResponseType(statusCode: 200, type: typeof(void))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult ChangePassword(Guid token, [FromBody] ChangePasswordDto passwordChanges)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                _appEntryService.ChangePassword(currentUserId, passwordChanges);
                return Ok();
            }
            catch(AuthenticationException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }

        /// <summary>
        /// Terminate all sessions
        /// </summary>
        /// <response code = "200">All sessions stopped</response>
        /// <response code = "400">Exception message</response>
        /// <response code = "401">Invalid token</response>
        [HttpDelete]
        [Route("exit/all/{token}")]
        [ProducesResponseType(statusCode: 200, type: typeof(void))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult ExitApp(Guid token)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                _appEntryService.FullExitApp(token, currentUserId);
                return Ok();
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
        /// Terminate current session
        /// </summary>
        /// <response code = "200">Current session stopped"</response>
        /// <response code = "400">Exception message</response>
        /// <response code = "401">Invalid token</response>
        [HttpDelete]
        [Route("exit/{token}")]
        [ProducesResponseType(statusCode: 200, type: typeof(void))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult FullExitApp(Guid token)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                _appEntryService.ExitApp(token, currentUserId);
                return Ok();
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
