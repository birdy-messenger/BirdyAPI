using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APItest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite.Internal.ApacheModRewrite;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APItest.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly UserContext _db;
        public LoginController(UserContext context)
        {
            _db = context;
        }
        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get([FromBody]User user)
        {
            var bdUser = _db.Users.First(k => k.Email == user.Email && k.PasswordHash == user.PasswordHash);
            if(bdUser != null)
                return Ok(new LoginAnswer {Id = bdUser.Id, Token = bdUser.Token});

            return BadRequest();
        }

        [HttpGet("all")]
        public IEnumerable<User> Get()
        {
            if (_db.Users.Any())
                return _db.Users.ToList();

            return new List<User>{new User {Email = "testLogin", PasswordHash = 0, Id = 0}};
        }
        // POST api/<controller>
        [HttpPost]
        public IActionResult Post([FromBody]User user)
        {
            if (_db.Users != null && _db.Users.Contains(user))
                return BadRequest();
            user.Token = new Random().Next(int.MaxValue/2, int.MaxValue);
            _db.Add(user);
            _db.SaveChanges();
            return Ok();
        }

    }
}
