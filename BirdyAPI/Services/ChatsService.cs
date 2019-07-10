using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.Dto;

namespace BirdyAPI.Services
{
    public class ChatsService
    {
        private readonly BirdyContext _context;
        public ChatsService(BirdyContext context)
        {
            _context = context;
        }

        public List<ChatInfoDto> GetAllChats(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
