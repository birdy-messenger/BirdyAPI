﻿using System;
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
        private readonly AuthService _authService;
        public AuthenticationController(UserContext context)
        {
            _authService = new AuthService(context);
        }

        // GET: api/<controller>

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
