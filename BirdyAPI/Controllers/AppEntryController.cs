using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.Dto;
using BirdyAPI.Services;
using BirdyAPI.Tools;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BirdyAPI.Controllers
{
    [Route("api/app")]
    public class AppEntryController : Controller
    {
        private readonly AppEntryService _appEntryService;

        public AppEntryController(UserContext context)
        {
            _appEntryService = new AppEntryService(context);
        }

        [HttpGet]
        [Route("auth")]
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
    }
}
