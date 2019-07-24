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
    [Route("api/dialogs")]
    public class DialogController : ExtendedController
    {
        private readonly DialogService _dialogService;
        private readonly ToolService _toolService;

        public DialogController(BirdyContext context)
        {
            _dialogService = new DialogService(context);
            _toolService = new ToolService(context);
        }

        /// <summary>
        /// Get all user dialogs
        /// </summary>
        /// <response code = "200">Return dialogs info</response>
        /// <response code = "500">Unexpected Exception (only for debug)</response>
        /// <response code = "401">Invalid token</response>
        [HttpGet]
        [ProducesResponseType(statusCode: 200, type: typeof(List<DialogInfoDto>))]
        [ProducesResponseType(statusCode: 500, type: typeof(ExceptionDto))]
        [ProducesResponseType(statusCode: 401, type: typeof(void))]
        public IActionResult GetChats([FromHeader] Guid token)
        {
            try
            {
                int currentUserId = _toolService.ValidateToken(token);
                return Ok(_dialogService.GetDialogs(currentUserId));
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
    }
}
