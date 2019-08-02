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
        private static readonly Random Random;
        static UserServiceTest()
        {
            Random = new Random();
        }
        private UserService GetUserService()
        {
            return new UserService(TestFactory.GetContext());
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
            BirdyContext context = TestFactory.GetContext();

            int randomUserId = Random.Next();
            string randomTag = TestFactory.GetRandomString();

            User user = new User
            {
                AvatarReference = "test",
                CurrentStatus = UserStatus.Confirmed,
                Email = TestFactory.GetRandomString(),
                FirstName = "test",
                Id = randomUserId,
                PasswordHash = "testPassword",
                RegistrationDate = DateTime.Now,
                UniqueTag = randomTag
            };

            context.Users.Add(user);
            context.SaveChanges();

            UserService userService = new UserService(context);
            Assert.Equal(randomUserId, userService.GetUserIdByUniqueTag(randomTag));
        }

        [Fact]
        public void SendUserId_UserInfo()
        {
            BirdyContext context = TestFactory.GetContext();
            
            int randomUserId = Random.Next();
            string randomTag = TestFactory.GetRandomString();
            DateTime regData = DateTime.Now;

            User user = new User
            {
                AvatarReference = "test",
                CurrentStatus = UserStatus.Confirmed,
                Email = TestFactory.GetRandomString(),
                FirstName = "test",
                Id = randomUserId,
                PasswordHash = "testPassword",
                RegistrationDate = regData,
                UniqueTag = randomTag
            };

            context.Users.Add(user);
            context.SaveChanges();

            UserService userService = new UserService(context);
            var searchedAccount = userService.GetUserInfo(randomUserId);
            var expected = (regData, randomTag);
            var actual = (searchedAccount.RegistrationDate, searchedAccount.UniqueTag);
            Assert.Equal(expected, actual);
        }

       
    }
}
