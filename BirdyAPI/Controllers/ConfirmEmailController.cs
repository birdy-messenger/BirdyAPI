using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.Models;
using BirdyAPI.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BirdyAPI.Controllers
{
    [Route("api/confirmemail")]
    public class ConfirmEmailController : Controller
    {
        private readonly ConfirmEmailService _registrationService;
        public ConfirmEmailController(UserContext context)
        {
            _registrationService = new ConfirmEmailService(context);
        }
        [HttpGet]
        public IActionResult EmailConfirming([FromQuery] int id)
        {
            string answer = _registrationService.GetUserConfirmed(id);
            if (answer.Contains("ErrorMessage"))
                return BadRequest(answer);
            else
                return Ok(answer);
        }
    }       
}
