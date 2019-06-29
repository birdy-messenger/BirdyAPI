using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Models;
using BirdyAPI.Services;
using BirdyAPI.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite.Internal.ApacheModRewrite;

namespace BirdyAPI.Controllers
{
    [Route("api/auth")]
    public class AuthenticationController : Controller
    {
        //TODO:9 dispose UserContext and all usage
        private readonly AuthService _authService;
        public AuthenticationController(UserContext context)
        {
            _authService = new AuthService(context);
        }
        // GET: api/<controller>

        //TODO:6 Don't use User model for transport login with password
        [HttpGet]
        public IActionResult UserAuthentication([FromQuery] AuthenticationDto user)
        {
            try
            {
                return Ok(_authService.Authentication(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }

        [HttpGet("all")]
        public IEnumerable<User> GetUsers()
        {
            return _authService.GetAllUsers();
        }
    }
}
