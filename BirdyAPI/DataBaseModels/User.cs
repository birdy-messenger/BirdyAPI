using System;
using System.ComponentModel.DataAnnotations;
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

        }

        public User()
        {
            
        }

        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string UniqueTag { get; set; }
        public string PasswordHash { get; set; }
        public DateTime RegistrationDate { get; set; }

        public UserStatus CurrentStatus { get; set; }
        public string AvatarReference { get; set; } = "А нету пока аватарок :C";
    }
}
