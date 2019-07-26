using System;
using System.Linq;
using System.Security.Authentication;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Tools.Exceptions;
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

        public int GetUserIdByUniqueTag(string uniqueTag)
        {
            User currentUser = _context.Users.SingleOrDefault(k => k.UniqueTag == uniqueTag);
            if(currentUser == null)
                throw new ArgumentException();

            return currentUser.Id;
        }
    }
}
