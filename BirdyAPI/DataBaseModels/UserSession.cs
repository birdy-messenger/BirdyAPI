using System;
using System.ComponentModel.DataAnnotations;

namespace BirdyAPI.DataBaseModels
{
    public class UserSession
    {
        [Key]
        public Guid Token { get ; set; }
        public int UserId { get; set; }

        public UserSession() { }

        private UserSession(Guid token, int userId)
        {
            Token = token;
            UserId = userId;
        }

        public static UserSession Create(int userId)
        {
            return new UserSession(Guid.NewGuid(), userId);
        }
    }
}
