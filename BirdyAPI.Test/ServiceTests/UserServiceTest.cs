using System;
using System.Linq;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Services;
using BirdyAPI.Types;
using Xunit;

namespace BirdyAPI.Test.ServiceTests
{
    public class UserServiceTest
    {
        private static string RandomString => TestFactory.GetRandomString();
        private static int RandomUserId => TestFactory.GetRandomInt();
        private static BirdyContext Context => TestFactory.GetContext();
        private UserService GetUserService()
        {
            return new UserService(Context);
        }
        [Fact]
        public void SendInvalidUserTag_ArgumentException()
        {
            UserService userService = GetUserService();

            Assert.Throws<ArgumentException>(() => userService.GetUserIdByUniqueTag(null));
        }
        [Fact]
        public void SendValidUserTag_UserId()
        {
            User user = GetRandomUser();

            BirdyContext context = Context;
            context.Users.Add(user);
            context.SaveChanges();

            UserService userService = new UserService(context);

            Assert.Equal(user.Id, userService.GetUserIdByUniqueTag(user.UniqueTag));
        }

        [Fact]
        public void SendUserId_UserInfo()
        {
            BirdyContext context = Context;

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
                AvatarReference = RandomString,
                CurrentStatus = UserStatus.Confirmed,
                Email = RandomString,
                FirstName = RandomString,
                Id = RandomUserId,
                PasswordHash = RandomString,
                RegistrationDate = DateTime.Now,
                UniqueTag = RandomString
            };
            return user;
        }



    }
}
