using System;
using System.Linq;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Services;
using BirdyAPI.Types;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace BirdyAPI.Test.ServiceTests
{
    public class FriendServiceTest
    {
        private static BirdyContext Context => TestFactory.GetContext();
        private static int RandomUserId => TestFactory.GetRandomInt();
        private static int RandomCount => TestFactory.GetRandomInt(1, 15);
        private static string RandomString => TestFactory.GetRandomString();
        private static DateTime CurrentDateTime => TestFactory.GetDateTime();
        [Fact]
        public void SendNotFriendId_False()
        {
            FriendService friendService = new FriendService(Context);
            Assert.False(friendService.IsItUserFriend(RandomUserId, RandomUserId));
        }
        [Fact]
        public void SendUserId_ListOfFriends()
        {
            BirdyContext context = Context;
            FriendService friendService = new FriendService(context);
            int testUserId = RandomUserId;
            int randomCount = RandomCount;

            for (int i = 0; i < randomCount; i++)
            {
                int friendId = RandomUserId;
                context.Friends.Add(new Friend {FirstUserID = testUserId, RequestAccepted = true, SecondUserID = friendId});
                context.Users.Add(new User
                {
                    AvatarReference = RandomString,
                    CurrentStatus = UserStatus.Confirmed,
                    Email = RandomString,
                    FirstName = RandomString,
                    PasswordHash = RandomString,
                    RegistrationDate = CurrentDateTime,
                    UniqueTag = RandomString,
                    Id = friendId
                });
            }

            context.SaveChanges();

            Assert.Equal(randomCount, friendService.GetFriends(testUserId).Count);
        }

        [Fact]
        public void SendFriendRequest_InsertRequestToDb()
        {
            BirdyContext context = Context;
            FriendService friendService = new FriendService(context);
            int firstUserId = RandomUserId;
            int secondUserId = RandomUserId;
            friendService.SendFriendRequest(firstUserId, secondUserId);

            Assert.NotNull(context.Friends.SingleOrDefault(k => k.FirstUserID == firstUserId && k.SecondUserID == secondUserId && !k.RequestAccepted));
        }
    }
}
