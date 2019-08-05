using System;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
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
            using (BirdyContext context = ContextFactory.GetContext())
            {
                UserService userService = new UserService(context);

                Assert.Throws<ArgumentException>(() => userService.GetUserIdByUniqueTag(null));
            }
        }
        [Fact]
        public void SendValidUserTag_UserId()
        {
            User user = DatabaseModelsFactory.GetRandomUser();

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.Users.Add(user);
                context.SaveChanges();

                UserService userService = new UserService(context);

                Assert.Equal(user.Id, userService.GetUserIdByUniqueTag(user.UniqueTag));
            }
        }

        [Fact]
        public void SendUserId_UserInfo()
        {
            User user = DatabaseModelsFactory.GetRandomUser();

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.Users.Add(user);
                context.SaveChanges();

                UserService userService = new UserService(context);

                UserAccountDto searchedAccount = userService.GetUserInfo(user.Id);

                var expected = (user.RegistrationDate, user.UniqueTag);
                var actual = (searchedAccount.RegistrationDate, searchedAccount.UniqueTag);

                Assert.Equal(expected, actual);
            }
        }
    }
}
