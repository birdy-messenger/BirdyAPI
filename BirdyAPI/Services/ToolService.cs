using System;
using System.Linq;
using System.Security.Authentication;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Types;

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
                throw new AuthenticationException();

            return currentSession.UserId;
        }

        public int GetUserIdByUniqueTag(string uniqueTag)
        {
            User currentUser = _context.Users.SingleOrDefault(k => k.UniqueTag == uniqueTag);
            if(currentUser == null)
                throw new ArgumentException();

            return currentUser.Id;
        }

        public void GetUserConfirmed(int userId)
        {
            User currentUser = _context.Users.Find(userId);
            currentUser.CurrentStatus = UserStatus.Confirmed;
            _context.Users.Update(currentUser);
            _context.SaveChanges();
        }
    }
}
