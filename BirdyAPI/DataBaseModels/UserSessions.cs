using System;
using System.ComponentModel.DataAnnotations;

namespace BirdyAPI.DataBaseModels
{
    public class UserSessions
    {
        public UserSessions() { }

        public UserSessions(int userId)
        {
            Token = Guid.NewGuid();
            UserId = userId;
        }
        [Key]
        public Guid Token { get; set; }
        public int UserId { get; set; } 
    }
}
