using System;
using System.ComponentModel.DataAnnotations;

namespace BirdyAPI.DataBaseModels
{
    public class UserSession
    {
        public UserSession() { }

        public UserSession(int userId)
        {
            Token = Guid.NewGuid();
            UserId = userId;
        }
        [Key]
        public Guid Token { get; set; }
        public int UserId { get; set; } 
    }
}
