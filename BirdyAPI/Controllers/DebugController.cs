using System;
using System.Collections.Generic;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Services;
using BirdyAPI.Tools;
using Microsoft.AspNetCore.Mvc;


namespace BirdyAPI.Controllers
{
    [Route("debug")]
    public class DebugController : ExtendedController
    {
        private readonly DebugService _debugService;

        public DebugController(BirdyContext context)
        {
            _debugService = new DebugService(context);
        }

        [HttpGet]
        [Route("show")]
        [Produces(typeof(List<User>))]
        public IActionResult GetUsers()
        {
            try
            {
                return Ok(_debugService.GetAllUsers());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }
    }
}
