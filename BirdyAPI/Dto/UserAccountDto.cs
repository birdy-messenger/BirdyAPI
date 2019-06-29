using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BirdyAPI.Dto
{
    public class UserAccountDto
    {
        public UserAccountDto(string firstName, string avatarReference)
        {
            FirstName = firstName;
            AvatarReference = avatarReference;
        }
        public string FirstName { get; set; }
        public string AvatarReference { get; set; }
    }
}
