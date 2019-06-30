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

        public AppEntryController(UserContext context, IConfiguration configuration)
        {
            _appEntryService = new AppEntryService(context, configuration);
        }

        [HttpGet]
        [Route("auth")]
        [Produces(typeof(UserSessionDto))]
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

        [HttpGet]
        [Route("reg")]
        [Produces(typeof(void))]
        public IActionResult UserRegistration([FromQuery]RegistrationDto user)
        {
            try
            {
                _appEntryService.CreateNewAccount(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }

        [HttpGet]
        [Route("confirm")]
        [Produces(typeof(UserStatus))]
        [ApiExplorerSettings(IgnoreApi = true)]
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
    }
}
