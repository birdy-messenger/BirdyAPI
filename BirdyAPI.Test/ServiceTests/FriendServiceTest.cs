using System;
using System.Linq;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Services;
using BirdyAPI.Test.Factories;
using BirdyAPI.Types;
using Xunit;

namespace BirdyAPI.Test.ServiceTests
{
    public class FriendServiceTest
    {
        [Fact]
        public void SendNotFriendId_False()
        {
            FriendService friendService = new FriendService(ContextFactory.GetContext());
            Assert.False(friendService.IsItUserFriend(RandomValuesFactory.GetRandomInt(),
                RandomValuesFactory.GetRandomInt()));
        }
        [Fact]
        public void SendUserId_ListOfFriends()
        {
            BirdyContext context = ContextFactory.GetContext();
            FriendService friendService = new FriendService(context);
            int testUserId = RandomValuesFactory.GetRandomInt();
            int randomCount = RandomValuesFactory.GetRandomInt();

            for (int i = 0; i < randomCount; i++)
            {
                int friendId = RandomValuesFactory.GetRandomInt();
                context.Friends.Add(new Friend {FirstUserID = testUserId, RequestAccepted = true, SecondUserID = friendId});
                context.Users.Add(new User
                {
                    AvatarReference = RandomValuesFactory.GetRandomString(),
                    CurrentStatus = UserStatus.Confirmed,
                    Email = RandomValuesFactory.GetRandomString(),
                    FirstName = RandomValuesFactory.GetRandomString(),
                    PasswordHash = RandomValuesFactory.GetRandomString(),
                    RegistrationDate = RandomValuesFactory.GetDateTime(),
                    UniqueTag = RandomValuesFactory.GetRandomString(),
                    Id = friendId
                });
            }

            context.SaveChanges();

            Assert.Equal(randomCount, friendService.GetFriends(testUserId).Count);
        }

        [Fact]
        public void SendFriendRequest_InsertRequestToDb()
        {
            BirdyContext context = ContextFactory.GetContext();
            FriendService friendService = new FriendService(context);
            int firstUserId = RandomValuesFactory.GetRandomInt();
            int secondUserId = RandomValuesFactory.GetRandomInt();
            friendService.SendFriendRequest(firstUserId, secondUserId);

            Assert.NotNull(context.Friends.SingleOrDefault(k =>
                k.FirstUserID == firstUserId && k.SecondUserID == secondUserId && !k.RequestAccepted));
        }

        [Fact]
        public void AcceptRequest_UpdateRequestDb()
        {
            int firstUserId = RandomValuesFactory.GetRandomInt();
            int secondUserId = RandomValuesFactory.GetRandomInt();

            BirdyContext context = ContextFactory.GetContext();
            FriendService friendService = new FriendService(context);

            context.Friends.Add(new Friend
                {FirstUserID = firstUserId, SecondUserID = secondUserId, RequestAccepted = false});
            context.SaveChanges();

            friendService.AcceptFriendRequest(firstUserId, secondUserId);

            Assert.NotNull(context.Friends.SingleOrDefault(k =>
                k.RequestAccepted && k.FirstUserID == firstUserId && k.SecondUserID == secondUserId));
        }

        [Fact]
        public void AcceptRequest_NullReferenceException()
        {
            BirdyContext context = ContextFactory.GetContext();
            FriendService friendService = new FriendService(context);
            Assert.Throws<NullReferenceException>(() =>
                friendService.AcceptFriendRequest(RandomValuesFactory.GetRandomInt(),
                    RandomValuesFactory.GetRandomInt()));
        }

        [Fact]
        public void DeleteFriend_UpdateRequestDb()
        {
            BirdyContext context = ContextFactory.GetContext();
            FriendService friendService = new FriendService(context);

            int firstUserId = RandomValuesFactory.GetRandomInt();
            int secondUserId = RandomValuesFactory.GetRandomInt();

            context.Friends.Add(new Friend
                {FirstUserID = firstUserId, SecondUserID = secondUserId, RequestAccepted = true});
            context.SaveChanges();

            friendService.DeleteFriend(firstUserId, secondUserId);
            Assert.NotNull(context.Friends.SingleOrDefault(k =>
                k.FirstUserID == firstUserId && k.SecondUserID == secondUserId && !k.RequestAccepted));
        }

        [Fact]
        public void DeleteFriend_FlipRequestDb()
        {
            BirdyContext context = ContextFactory.GetContext();
            FriendService friendService = new FriendService(context);

            int firstUserId = RandomValuesFactory.GetRandomInt();
            int secondUserId = RandomValuesFactory.GetRandomInt();

            context.Friends.Add(new Friend
                { FirstUserID = firstUserId, SecondUserID = secondUserId, RequestAccepted = true });
            context.SaveChanges();

            friendService.DeleteFriend(secondUserId, firstUserId);
            Assert.NotNull(context.Friends.SingleOrDefault(k =>
                k.FirstUserID == secondUserId && k.SecondUserID == firstUserId && !k.RequestAccepted));
        }
    }
}
