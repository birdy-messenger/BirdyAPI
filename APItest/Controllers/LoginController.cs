using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APItest.Models;
using APItest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite.Internal.ApacheModRewrite;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APItest.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly UserContext _db;
        private readonly LoginService _loginService;
        public LoginController(UserContext context)
        {
            _db = context;
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
            if (_db.Users.Any())
                return _db.Users.ToList();

            return new List<User>{new User {Email = "testLogin", PasswordHash = 0, Id = 0}};
        }

        

    }
}
