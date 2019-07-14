using System;
using System.ComponentModel.DataAnnotations;
using BirdyAPI.Types;

namespace BirdyAPI.DataBaseModels
{
    //TODO :4 fix set in dbModels, models and Dto
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
    }
}
