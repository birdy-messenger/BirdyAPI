using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.Models;
using BirdyAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite.Internal.ApacheModRewrite;

namespace BirdyAPI.Controllers
{
    [Route("api/auth")]
    public class AuthenticationController : Controller
    {
        private readonly LoginService _loginService;
        public AuthenticationController(UserContext context)
        {
            _loginService = new LoginService(context);
        }
        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get([FromQuery] User user)
        {
            string answer = _loginService.Authentication(user);
            if (!answer.Contains("ErrorMessage"))
                return Ok(answer);
            else
                return BadRequest(answer);
        }

        [HttpGet("all")]
        public IEnumerable<User> Get()
        {
            return _loginService.GetAllUsers();
        }
    }
}
