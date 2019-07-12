﻿using System;
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
        private readonly IConfiguration _configuration;

        public UserService(BirdyContext context, IConfiguration configuration)   
        {
            _context = context;
            _configuration = configuration;
        }

        public UserAccountDto SearchUserInfo(int userId)
        {
            User user = _context.Users.FirstOrDefault(k => k.Id == userId);
            if (user == null)
                throw new ArgumentException("User Not Found");
            else
            {
                return new UserAccountDto
                {
                    AvatarReference = user.AvatarReference,
                    FirstName = user.FirstName,
                    UniqueTag = user.UniqueTag
                };
            }
        }
        

        
        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
    }
}
