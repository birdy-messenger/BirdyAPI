using System;
using System.Collections.Generic;
using System.Text;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Types;

namespace BirdyAPI.Test.Factories
{
    public static class DatabaseModelsFactory
    {
        public static User GetRandomUser()
        {
            User user = new User
            {
                AvatarReference = RandomValuesFactory.GetRandomString(),
                CurrentStatus = UserStatus.Confirmed,
                Email = RandomValuesFactory.GetRandomString(),
                FirstName = RandomValuesFactory.GetRandomString(),
                Id = RandomValuesFactory.GetRandomInt(),
                PasswordHash = RandomValuesFactory.GetRandomString(),
                RegistrationDate = DateTime.Now,
                UniqueTag = RandomValuesFactory.GetRandomString()
            };
            return user;
        }
    }
}
