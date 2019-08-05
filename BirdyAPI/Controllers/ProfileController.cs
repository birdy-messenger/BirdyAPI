using System;
using System.Data;
using System.Security.Authentication;
using BirdyAPI.Dto;
using BirdyAPI.Services;
using BirdyAPI.Tools.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BirdyAPI.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("profile")]
    public class ProfileController : ExtendedController
    {
        private readonly ProfileService _profileService;
        private readonly AppEntryService _appEntryService;

        public ProfileController(BirdyContext context)
        {
            _profileService = new ProfileService(context);
            _appEntryService = new AppEntryService(context);
        }


        /// <summary>
        /// Set user avatar
        /// </summary>
        /// <response code = "200">Return reference to avatar</response>
        /// <response code = "500">Unexpected Exception (only for debug)</response>
        /// <response code = "401">Invalid token</response>
        [HttpPut]
        [Route("avatar")]
        [ProducesResponseType(statusCode: 200, type: typeof(SimpleAnswerDto))]
        [ProducesResponseType(statusCode: 500, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult SetAvatar([FromHeader] Guid token, [FromBody] byte[] photoBytes)
        {
            try
            {
                int currentUserId = ValidateToken(token);
                return Ok(_profileService.SetAvatar(currentUserId, photoBytes));
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Set user unique tag
        /// </summary>
        /// <response code = "200">Tag changed</response>
        /// <response code = "500">Unexpected Exception (only for debug)</response>
        /// <response code = "401">Invalid token</response>
        /// <response code = "403">Tag is not unique</response>
        [HttpPut]
        [Route("uniqueTag")]
        [ProducesResponseType(statusCode: 200, type: typeof(SimpleAnswerDto))]
        [ProducesResponseType(statusCode: 500, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        [ProducesResponseType(statusCode: 403, type: typeof(void))]
        public IActionResult ChangeUniqueTag([FromHeader] Guid token, [FromBody] string uniqueTag)
        {
            try
            {
                int currentUserId = ValidateToken(token);
                _profileService.SetUniqueTag(currentUserId, uniqueTag);
                return Ok();
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (DuplicateNameException)
            {
                return Forbid();
            }
        }
    }
}
