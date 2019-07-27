using System;
using System.ComponentModel.DataAnnotations;

namespace BirdyAPI.DataBaseModels
{
    public class ConfirmToken
    {
        [Key]
        public string Email { get; set; }
        public Guid Token { get; set; }
        public DateTime TokenDate { get; set; }
    }
}
