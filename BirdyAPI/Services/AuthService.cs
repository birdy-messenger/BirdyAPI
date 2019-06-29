using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Models;
using BirdyAPI.Tools;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BirdyAPI.Services
{
    public class AuthService
    {
        private readonly UserContext _context;

        public AuthService(UserContext context)
        {
            _context = context;
        }
        public UserSessionDto Authentication(AuthenticationDto user)
        {
            User currentUser = _context.Users.SingleOrDefault(k => k.Email == user.Email && k.PasswordHash == user.PasswordHash);
            if (currentUser != null)
            {
                if (currentUser.CurrentStatus == UserStatus.Unconfirmed)
                    throw new Exception("Need to confirm email");
                else
                    return new UserSessionDto(currentUser.Id, currentUser.Token);
            }

            throw new ArgumentException("Invalid email or password");
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }   
    }
}
