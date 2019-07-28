using System;
using BirdyAPI.DataBaseModels;

namespace BirdyAPI.Dto
{
    public class UserAccountDto
    {
        public string FirstName { get; set; }
        public string AvatarReference { get; set; }
        public string UniqueTag { get; set; }
        public DateTime RegistrationDate { get; set; }
        public UserAccountDto() { }

        private UserAccountDto(string firstName, string avatarReference, string uniqueTag, DateTime registrationDate)
        {
            FirstName = firstName;
            AvatarReference = avatarReference;
            UniqueTag = uniqueTag;
            RegistrationDate = registrationDate;
        }

        public static UserAccountDto Create(User user)
        {
            return new UserAccountDto(user.FirstName, user.AvatarReference, user.UniqueTag, user.RegistrationDate);
        }
    }
}
