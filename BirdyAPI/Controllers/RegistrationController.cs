using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.Answers;
using BirdyAPI.Models;
using BirdyAPI.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BirdyAPI.Controllers
{
    [Route("api/user.reg")]
    public class RegistrationController : Controller
    {
        private readonly RegistrationService _registrationService;
        public RegistrationController(UserContext context)
        {
            _registrationService = new RegistrationService(context);
        }
        // POST api/<controller>
        [HttpGet]
        public IActionResult Get([FromQuery]User user)
        {
            RegistrationAnswer answer = _registrationService.CreateNewAccount(user);
            if (answer.ErrorMessage == null)
                return BadRequest(answer);
            else
                return Ok(answer);
        }
    }
}
