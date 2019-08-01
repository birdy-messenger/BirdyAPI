using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Services;
using BirdyAPI.Types;
using Xunit;

namespace BirdyAPI.Test
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
            return new UserService(TestContext.GetContext());
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
            BirdyContext context = TestContext.GetContext();

            int randomUserId = Random.Next();
            string randomTag = GetRandomString(8);

            User user = new User
            {
                AvatarReference = "test",
                CurrentStatus = UserStatus.Confirmed,
                Email = GetRandomString(7),
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
            BirdyContext context = TestContext.GetContext();

            int randomUserId = Random.Next();
            string randomTag = GetRandomString(8);
            DateTime regData = DateTime.Now;

            User user = new User
            {
                AvatarReference = "test",
                CurrentStatus = UserStatus.Confirmed,
                Email = GetRandomString(7),
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

        private string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}
