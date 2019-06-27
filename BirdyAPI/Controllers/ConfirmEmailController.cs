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
        public IActionResult Get([FromQuery] int id)
        {
            _registrationService.GetUserConfirmed(id);
            return Ok();
        }
    }       
}
