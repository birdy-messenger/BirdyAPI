using System;
using System.Security.Authentication;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Services;
using BirdyAPI.Tools;
using Microsoft.AspNetCore.Mvc;

namespace BirdyAPI.Controllers
{
    [Route("api/messages")]
    public class MessageController : Controller
    {
        private readonly MessageService _messageService;
        private readonly ToolService _toolService;

        public MessageController(BirdyContext context)
        {
            _messageService = new MessageService(context);
            _toolService = new ToolService(context);
        }
    }
}
