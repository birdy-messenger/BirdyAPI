using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.Models;
using BirdyAPI.Answers;
using Microsoft.AspNetCore.Mvc;

namespace BirdyAPI.Services
{
    public class LoginService
    {
        private readonly UserContext _context;

        public LoginService(UserContext context)
        {
            _context = context;
        }
        public LoginAnswer Authentication(User user)
        {
            var currentUser = _context.Users.FirstOrDefault(k => k.Email == user.Email && k.PasswordHash == user.PasswordHash);
            if (currentUser != null)
                return new LoginAnswer { Id = currentUser.Id, Token = currentUser.Token};

            return new LoginAnswer {ErrorMessage = "Invalid email or password"};
        }

        public List<User> GetAllUsers()
        {
            if (_context.Users.Any())
                return _context.Users.ToList();

            return new List<User> { new User { Email = "testLogin", PasswordHash = 0, Id = 0 } };
        }
    }
}
