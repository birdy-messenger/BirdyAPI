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
        //TODO:9 dispose UserContext and all usage
        private readonly LoginService _loginService;
        public AuthenticationController(UserContext context)
        {
            _loginService = new LoginService(context);
        }
        // GET: api/<controller>

        //TODO:6 Don't use User model for transport login with password
        [HttpGet]
        public IActionResult UserAuthentication([FromQuery] User user)
        {
            string answer = _loginService.Authentication(user);
            if (!answer.Contains("ErrorMessage"))
                return Ok(answer);
            else
                return BadRequest(answer);
        }

        [HttpGet("all")]
        public IEnumerable<User> GetUsers()
        {
            return _loginService.GetAllUsers();
        }
    }
}
