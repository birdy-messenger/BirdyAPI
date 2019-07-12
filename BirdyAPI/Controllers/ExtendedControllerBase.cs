using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BirdyAPI.Controllers
{
    public class ExtendedController : Controller
    {
        protected ObjectResult InternalServerError(ExceptionDto exception)
        {
            return StatusCode(500, exception);
        }
    }
}
