using System;
using System.ComponentModel.DataAnnotations;

namespace BirdyAPI.DataBaseModels
{
    public class UserSession
    {
        [Key]
        public Guid Token { get ; set; }
        public int UserId { get; set; } 
    }
}
