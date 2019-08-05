using System;
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

        protected int ValidateToken(Guid token)
        {
            return _accessService.ValidateToken(token);
        }

        protected void CheckChatAccess(Guid chatId, int userId, ChatStatus status)
        {
            _accessService.CheckChatUserAccess(chatId, userId, status);
        }

        protected bool CheckUniqueTagAvailable(string uniqueTag)
        {
            return _accessService.CheckUniqueTagAvailable(uniqueTag);
        }
    }
}
