using System;
using System.Collections.Generic;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Models;
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

        public AppEntryController(BirdyContext context, IConfiguration configuration)
        {
            _appEntryService = new AppEntryService(context, configuration);
        }

        [HttpGet]
        [Route("auth")]
        [ProducesResponseType(statusCode:200, type:typeof(UserSessionDto))]
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
        [ProducesResponseType(statusCode: 200, type: typeof(string))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
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
        public IActionResult ChangePassword([FromQuery] int id, [FromBody] string oldPasswordHash, [FromBody] string newPasswordHash)
        {
            try
            {
                return Ok(_appEntryService.ChangePassword(id, oldPasswordHash, newPasswordHash));
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
        public IActionResult ExitApp([FromQuery] UserSessions currentSession)
        {
            try
            {
                return Ok(_appEntryService.ExitApp(currentSession));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }
    }
}
