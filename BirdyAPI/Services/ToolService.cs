using System;
using System.Linq;
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

        public int GetUserIdByUniqueTag(string uniqueTag)
        {
            User currentUser = _context.Users.SingleOrDefault(k => k.UniqueTag == uniqueTag);
            if(currentUser == null)
                throw new Exception();

            return currentUser.Id;
        }
    }
}
