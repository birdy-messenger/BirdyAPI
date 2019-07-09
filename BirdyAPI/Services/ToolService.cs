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
                return false;
            else if (currentSession.Token != token)
                return false;
            else
                return true;
        }
    }
}
