using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
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

        public void ValidateToken(int userId, Guid token)
        {
            UserSessions currentSession = _context.UserSessions.Find(token);
            if (currentSession == null)
                throw new AuthenticationException("Invalid session");
            else if (currentSession.UserId != userId)
                throw new AuthenticationException("Invalid token");
        }

        public void ValidateToken(UserSessions session)
        {
            ValidateToken(session.UserId, session.Token);
        }
    }
}
