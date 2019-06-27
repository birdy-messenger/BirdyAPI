using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BirdyAPI.Services
{
    public class LoginService
    {
        private readonly UserContext _context;

        public LoginService(UserContext context)
        {
            _context = context;
        }
        public string Authentication(User user)
        {
            var currentUser = _context.Users.FirstOrDefault(k => k.Email == user.Email && k.PasswordHash == user.PasswordHash);
            if (currentUser != null)
                return JsonConvert.SerializeObject(new { Id = currentUser.Id, Token = currentUser.Token});

            return JsonConvert.SerializeObject(new {ErrorMessage = "Invalid email or password"});
        }

        public List<User> GetAllUsers()
        {
            if (_context.Users.Any())
                return _context.Users.ToList();

            return new List<User> { new User { Email = "testLogin", PasswordHash = "TEST", Id = 0 } };
        }
    }
}
