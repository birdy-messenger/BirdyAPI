using System;
using System.Security.Authentication;
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

        public int ValidateToken(Guid token)
        {
            UserSession currentSession = _context.UserSessions.Find(token);
            if (currentSession == null)
                throw new AuthenticationException("Invalid token");
            else
                return currentSession.UserId;
        }
    }
}
