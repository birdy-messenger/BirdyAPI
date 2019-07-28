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
        public ConfirmToken() { }

        private ConfirmToken(string email, DateTime creationTime, Guid token)
        {
            Email = email;
            Token = token;
            TokenDate = creationTime;
        }
        public static ConfirmToken Create(string email)
        {
            return new ConfirmToken(email, DateTime.Now, Guid.NewGuid());
        }
    }
}
