using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.Models;

namespace BirdyAPI.Services
{
    public class ConfirmEmailService
    {
        private readonly UserContext _context;

        public ConfirmEmailService(UserContext context)
        {
            _context = context;
        }

        public void GetUserConfirmed(int id)
        {
            User user = _context.Users.Find(id);
            user.CurrentStatus = UserStatus.Status.Confirmed;
            
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
