using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BirdyAPI.Controllers
{
    [Route("api/chats")]
    public class ChatsController : Controller
    {
        private readonly ChatsService _chatsService;
        public ChatsController(BirdyContext context)
        {
            _chatsService = new ChatsService(context);
        }
    }
}
