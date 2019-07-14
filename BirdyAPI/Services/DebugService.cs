using System.Collections.Generic;
using System.Linq;
using BirdyAPI.DataBaseModels;

namespace BirdyAPI.Services
{
    public class DebugService
    {
        private readonly BirdyContext _context;

        public DebugService(BirdyContext context)
        {
            _context = context;
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
    }
}
