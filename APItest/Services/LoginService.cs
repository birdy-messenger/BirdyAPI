using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APItest.Models;
using Microsoft.AspNetCore.Mvc;

namespace APItest.Services
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
            var currentUser = _context.Users.First(k => k.Email == user.Email && k.PasswordHash == user.PasswordHash);
            if (currentUser != null)
                return new LoginAnswer { Id = currentUser.Id, Token = currentUser.Token};

            return new LoginAnswer {ErrorMessage = "Invalid email or password"};
        }
    }
}
