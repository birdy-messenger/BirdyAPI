using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using BirdyAPI.Dto;
using BirdyAPI.Models;

namespace BirdyAPI.DataBaseModels
{
    public class User
    {
        public User(RegistrationDto registrationData)
        {
            Email = registrationData.Email;
            FirstName = registrationData.FirstName;
            PasswordHash = registrationData.PasswordHash;
            RegistrationDate = DateTime.Now;
            Token = new Random().Next(int.MaxValue / 2, int.MaxValue);
            CurrentStatus = UserStatus.Unconfirmed;
        }

        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string PasswordHash { get; set; }
        public int Token { get; set; }
        public DateTime RegistrationDate { get; set; }

        public UserStatus CurrentStatus { get; set; }
        public string AvatarReference { get; set; } = "А нету пока аватарок :C";
    }
}
