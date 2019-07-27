using System;
using System.Collections.Generic;
using System.Security.Authentication;
using BirdyAPI.Dto;
using BirdyAPI.Services;
using BirdyAPI.Tools.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BirdyAPI.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("dialogs")]
    public class DialogController : ExtendedController
    {
        private readonly DialogService _dialogService;
        private readonly UserService _userService;

        public DialogController(BirdyContext context) : base(context)
        {
            _dialogService = new DialogService(context);
            _userService = new UserService(context);
        }

        /// <summary>
        /// Get all user dialogs
        /// </summary>
        /// <response code = "200">Return dialogs info</response>
        /// <response code = "500">Unexpected Exception (only for debug)</response>
        /// <response code = "401">Invalid token</response>
        [HttpGet]
        [ProducesResponseType(statusCode: 200, type: typeof(List<DialogPreviewDto>))]
        [ProducesResponseType(statusCode: 500, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult GetDialogs([FromHeader] Guid token)
        {
            try
            {
                int currentUserId = ValidateToken(token);
                return Ok(_dialogService.GetDialogsPreview(currentUserId));
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.SerializeAsResponse());
            }
        }

        /// <summary>
        /// Get dialog info
        /// </summary>
        /// <response code = "200">Return dialogs info</response>
        /// <response code = "500">Unexpected Exception (only for debug)</response>
        /// <response code = "401">Invalid token</response>
        /// <response code = "404">User not found</response>
        [HttpGet]
        [Route("{interlocutorUniqueTag}")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<DialogPreviewDto>))]
        [ProducesResponseType(statusCode: 500, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult GetDialog([FromHeader] Guid token, string interlocutorUniqueTag)
        {
            try
            {
                int currentUserId = ValidateToken(token);
                int interlocutorId = _userService.GetUserIdByUniqueTag(interlocutorUniqueTag);
                return Ok(_dialogService.GetDialog(currentUserId, interlocutorId));
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.SerializeAsResponse());
            }
        }
    }
}
