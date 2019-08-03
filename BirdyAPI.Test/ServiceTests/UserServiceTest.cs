using System;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Services;
using BirdyAPI.Test.Factories;
using BirdyAPI.Types;
using Xunit;

namespace BirdyAPI.Test.ServiceTests
{
    public class UserServiceTest
    {
        [Fact]
        public void SendInvalidUserTag_ArgumentException()
        {
            UserService userService = new UserService(ContextFactory.GetContext());

            Assert.Throws<ArgumentException>(() => userService.GetUserIdByUniqueTag(null));
        }
        [Fact]
        public void SendValidUserTag_UserId()
        {
            User user = GetRandomUser();

            BirdyContext context = ContextFactory.GetContext();
            context.Users.Add(user);
            context.SaveChanges();

            UserService userService = new UserService(context);

            Assert.Equal(user.Id, userService.GetUserIdByUniqueTag(user.UniqueTag));
        }

        [Fact]
        public void SendUserId_UserInfo()
        {
            BirdyContext context = ContextFactory.GetContext();

            User user = GetRandomUser();

            context.Users.Add(user);
            context.SaveChanges();

            UserService userService = new UserService(context);

            var searchedAccount = userService.GetUserInfo(user.Id);
            var expected = (user.RegistrationDate, user.UniqueTag);
            var actual = (searchedAccount.RegistrationDate, searchedAccount.UniqueTag);

            Assert.Equal(expected, actual);
        }

        private User GetRandomUser()
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
