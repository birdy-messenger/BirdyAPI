using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace BirdyAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string PasswordHash { get; set; }
        public int Token { get; set; }
        public DateTime RegistrationDate { get; set; }

        public UserStatus.Status CurrentStatus { get; set; }
        public string AvatarReference { get; set; } = "А нету пока аватарок :C";
    }
}
