using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.Models;

namespace BirdyAPI.Services
{
    public class GetUserService
    {
        private readonly UserContext _context;

        public GetUserService(UserContext context)
        {
            _context = context;
        }

        public User SearchUserInfo(int id)
        {
            return _context.Users.FirstOrDefault(k => k.Id == id); 
        }
    }
}
