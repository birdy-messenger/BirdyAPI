using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.Answers;
using BirdyAPI.Models;
using BirdyAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite.Internal.ApacheModRewrite;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BirdyAPI.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly LoginService _loginService;
        public AuthController(UserContext context)
        {
            _loginService = new LoginService(context);
        }
        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get([FromQuery]User user)
        {
            LoginAnswer answer = _loginService.Authentication(user);
            if (answer.ErrorMessage == null)
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
