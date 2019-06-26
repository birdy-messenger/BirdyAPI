using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.Models;

namespace BirdyAPI.Services
{
    public class RegistrationService
    {
        private readonly UserContext _context;

        public RegistrationService(UserContext context)
        {
            _context = context;
        }

        public void CreateNewAccount(User user)
        {
            if (_context.Users != null && _context.Users.Contains(user))
                return;
            user.Token = new Random().Next(int.MaxValue / 2, int.MaxValue);
            _context.Add(user);
            _context.SaveChanges();
        }
    }
}
