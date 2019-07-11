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
    [Route("api/app")]
    public class AppEntryController : Controller
    {
        private readonly AppEntryService _appEntryService;
        private readonly ToolService _toolService;

        public AppEntryController(BirdyContext context, IConfiguration configuration)
        {
            _appEntryService = new AppEntryService(context, configuration);
            _toolService = new ToolService(context);
        }

        //TODO :1 Decide on formats [FromBody], [FromQuery] in arguments
        [HttpGet]
        [Route("auth")]
        [ProducesResponseType(statusCode:200, type:typeof(UserSession))]
        [ProducesResponseType(statusCode:400, type: typeof(ExceptionDto))]
        public IActionResult UserAuthentication([FromQuery] AuthenticationDto user)
        {
            try
            {
                return Ok(_appEntryService.Authentication(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }

        [HttpPost]
        [Route("reg")]
        [ProducesResponseType(statusCode: 200, type: typeof(SimpleAnswerDto))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        public IActionResult UserRegistration([FromQuery]RegistrationDto user)
        {
            try
            {
                return Ok(_appEntryService.CreateNewAccount(user));
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

        [HttpPut]
        [Route("changePassword")]
        [ProducesResponseType(statusCode: 200, type: typeof(SimpleAnswerDto))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult ChangePassword([FromQuery] Guid token, [FromBody] string oldPasswordHash, [FromBody] string newPasswordHash)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                return Ok(_appEntryService.ChangePassword(currentUserId, oldPasswordHash, newPasswordHash));
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

        [HttpDelete]
        [Route("exit")]
        [ProducesResponseType(statusCode: 200, type: typeof(SimpleAnswerDto))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult ExitApp([FromQuery] Guid token)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                return Ok(_appEntryService.ExitApp(token, currentUserId));
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

        [HttpDelete]
        [Route("exitAll")]
        [ProducesResponseType(statusCode: 200, type: typeof(SimpleAnswerDto))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult FullExitApp([FromQuery] Guid token)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                return Ok(_appEntryService.ExitApp(token, currentUserId));
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
