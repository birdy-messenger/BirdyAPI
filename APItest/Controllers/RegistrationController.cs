using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APItest.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APItest.Controllers
{
    [Route("api/[controller]")]
    public class RegistrationController : Controller
    {
        private readonly UserContext _db;
        // POST api/<controller>
        [HttpPost]
        public IActionResult Post([FromBody]User user)
        {
            if (_db.Users != null && _db.Users.Contains(user))
                return BadRequest();
            user.Token = new Random().Next(int.MaxValue / 2, int.MaxValue);
            _db.Add(user);
            _db.SaveChanges();
            return Ok();
        }
    }
}
