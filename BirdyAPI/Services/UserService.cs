using System;
using System.Collections.Generic;
using System.Linq;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;

namespace BirdyAPI.Services
{
    public class UserService
    {
        private readonly UserContext _context;

        public UserService(UserContext context)
        {
            _context = context;
        }

        public UserAccountDto SearchUserInfo(UserSessionDto user)
        {
            User currentUser = _context.Users.FirstOrDefault(k => k.Id == user.Id);
            if (currentUser == null)
                throw new ArgumentException("User Not Found");
            else
            {
                if (IsTokenValid(currentUser, user.Token))
                    return new UserAccountDto(currentUser.FirstName, currentUser.AvatarReference);
                else
                    throw new ArgumentException("Invalid Token");
            }
        }
        private bool IsTokenValid(User user, int token)
        {
            if (user.Token == token)
                return true;
            else
                return false;
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
    }
}
