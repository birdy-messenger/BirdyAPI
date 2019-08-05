using System;
using BirdyAPI.Dto;
using BirdyAPI.Services;
using BirdyAPI.Types;
using Microsoft.AspNetCore.Mvc;

namespace BirdyAPI.Controllers
{
    public class ExtendedController : Controller
    {
        private readonly AccessService _accessService;
        public ExtendedController(BirdyContext context)
        {
            _accessService = new AccessService(context);
        }
        public ExtendedController() { }
        protected ObjectResult InternalServerError(ExceptionDto exception)
        {
            return StatusCode(500, exception);
        }

        protected ObjectResult PartialContent(SimpleAnswerDto answer)
        {
            return StatusCode(206, answer);
        }

        protected int ValidateToken(Guid token)
        {
            return _accessService.ValidateToken(token);
        }

        protected void CheckChatAccess(Guid chatId, int userId, ChatStatus status)
        {
            _accessService.CheckChatUserAccess(chatId, userId, status);
        }
    }
}
