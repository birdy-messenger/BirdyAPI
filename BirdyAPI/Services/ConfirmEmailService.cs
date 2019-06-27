using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.Models;
using Newtonsoft.Json;

namespace BirdyAPI.Services
{
    public class ConfirmEmailService
    {
        private readonly UserContext _context;

        public ConfirmEmailService(UserContext context)
        {
            _context = context;
        }

        public string GetUserConfirmed(int id)
        {
            User user = _context.Users.Find(id);
            if (user == null)
                return JsonConvert.SerializeObject(new {ErrorMessage = "Invalid link"});

            user.CurrentStatus = UserStatus.Confirmed;
            
            _context.Users.Update(user);
            _context.SaveChanges();
            return JsonConvert.SerializeObject(new {Status = user.CurrentStatus});
        }
    }
}
