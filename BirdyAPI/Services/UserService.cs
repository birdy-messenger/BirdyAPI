using System;
using System.Collections.Generic;
using System.Linq;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;

namespace BirdyAPI.Services
{
    public class UserService
    {
        private readonly BirdyContext _context;

        public UserService(BirdyContext context)   
        {
            _context = context;
        }
        public UserAccountDto GetUserInfo(int userId)
        {
            User user = _context.Users.Find(userId);

            return new UserAccountDto
            {
                AvatarReference = user.AvatarReference,
                FirstName = user.FirstName,
                UniqueTag = user.UniqueTag,
                RegistrationDate = user.RegistrationDate
            };
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
    }
}
