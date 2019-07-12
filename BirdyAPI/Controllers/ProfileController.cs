using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using BirdyAPI.Dto;
using BirdyAPI.Services;
using BirdyAPI.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BirdyAPI.Controllers
{
    [Route("api/[controller]")]
    public class ProfileController : Controller
    {
        private readonly ToolService _toolService;
        private readonly ProfileService _profileService;

        public ProfileController(BirdyContext context, IConfiguration configuration)
        {
            _toolService = new ToolService(context);
            _profileService = new ProfileService(context, configuration);
        }

        [HttpPost]
        [Route("setAvatar")]
        [ProducesResponseType(statusCode: 200, type: typeof(SimpleAnswerDto))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult SetAvatar([FromQuery] Guid token, [FromBody] byte[] photoBytes)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                return Ok(_profileService.SetProfileAvatar(currentUserId, photoBytes));
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.SerializeAsResponse());
            }
        }
    }
}
