using System;
using System.Security.Authentication;
using BirdyAPI.Dto;
using BirdyAPI.Services;
using BirdyAPI.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BirdyAPI.Controllers
{
    [Route("profile")]
    public class ProfileController : Controller
    {
        private readonly ToolService _toolService;
        private readonly ProfileService _profileService;

        public ProfileController(BirdyContext context, IConfiguration configuration)
        {
            _toolService = new ToolService(context);
            _profileService = new ProfileService(context, configuration);
        }


        /// <summary>
        /// Set user avatar
        /// </summary>
        /// <response code = "200">Return reference to avatar</response>
        /// <response code = "400">Exception message</response>
        /// <response code = "401">Invalid token</response>
        [HttpPut]
        [Route("avatar")]
        [ProducesResponseType(statusCode: 200, type: typeof(SimpleAnswerDto))]
        [ProducesResponseType(statusCode: 400, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult SetAvatar([FromHeader] Guid token, [FromBody] byte[] photoBytes)
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
