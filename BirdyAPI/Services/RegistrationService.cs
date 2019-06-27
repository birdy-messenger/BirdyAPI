using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.Models;
using Newtonsoft.Json;

namespace BirdyAPI.Services
{
    public class RegistrationService
    {
        private readonly UserContext _context;

        public RegistrationService(UserContext context)
        {
            _context = context;
        }

        public string CreateNewAccount(User user)
        {
            if (_context.Users?.FirstOrDefault(k => k.Email == user.Email) != null)
                return JsonConvert.SerializeObject(new {ErrorMessage = "Duplicate account"});
            user.Token = new Random().Next(int.MaxValue / 2, int.MaxValue);
            _context.Add(user);
            _context.SaveChanges();
            return JsonConvert.SerializeObject(new { FirstName = user.FirstName});
        }
    }
}
