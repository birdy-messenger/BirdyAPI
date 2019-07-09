using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.DataBaseModels;

namespace BirdyAPI.Services
{
    public class ToolService
    {
        private readonly BirdyContext _context;

        public ToolService(BirdyContext context)
        {
            _context = context;
        }

        private bool IsTokenValid(int userId, Guid token)
        {
            UserSessions currentSession = _context.UserSessions.Find(token);
            if (currentSession == null)
                throw new Exception("Invalid session");
            else if (currentSession.UserId != userId)
                throw new Exception("Invalid token");
            else
                return true;
        }
    }
}
