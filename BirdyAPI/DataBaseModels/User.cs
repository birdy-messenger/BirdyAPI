using System;
using System.ComponentModel.DataAnnotations;
using BirdyAPI.Dto;
using BirdyAPI.Types;

namespace BirdyAPI.DataBaseModels
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string UniqueTag { get; set; }
        public string PasswordHash { get; set; }
        public DateTime RegistrationDate { get; set; }

        public UserStatus CurrentStatus { get; set; }
        public string AvatarReference { get; set; }
        public User() { }

        private User(string email, string firstName, string passwordHash)
        {
            Email = email;
            PasswordHash = passwordHash;
            FirstName = firstName;
            UniqueTag = null;
            RegistrationDate = DateTime.Now;
            CurrentStatus = UserStatus.Unconfirmed;
            AvatarReference = null; //Нада дефолтную какую-нибудь сделать
        }

        public static User Create(RegistrationDto registrationData)
        {
            return new User(registrationData.Email, registrationData.FirstName, registrationData.PasswordHash);
        }
    }
}
