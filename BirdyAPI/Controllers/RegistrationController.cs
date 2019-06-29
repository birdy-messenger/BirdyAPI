using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Models;
using BirdyAPI.Services;
using BirdyAPI.Tools;
using Microsoft.AspNetCore.Mvc;

namespace BirdyAPI.Controllers
{
    [Route("api/user/reg")]
    public class RegistrationController : Controller
    {
        private readonly RegistrationService _registrationService;
        public RegistrationController(UserContext context)
        {
            _registrationService = new RegistrationService(context);
        }

        [HttpGet]
        public IActionResult UserRegistration([FromQuery]RegistrationDto user)
        {
            try
            {
                return Ok(_registrationService.CreateNewAccount(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }
    }
}
