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
    [Route("api/user.get")]
    public class UserGetController : Controller
    {
        private readonly GetUserService _getUserService;

        public UserGetController(UserContext context)
        {
            _getUserService = new GetUserService(context);
        }
        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get([FromQuery]int id, int token)
        {
            User currentUser = _getUserService.SearchUserInfo(id);
            if (currentUser == null)
                return BadRequest();
            else
                return Ok(currentUser);
        }
    }
}
