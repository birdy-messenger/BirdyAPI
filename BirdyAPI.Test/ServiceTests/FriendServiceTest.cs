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
            using (BirdyContext context = ContextFactory.GetContext())
            {
                FriendService friendService = new FriendService(context);
                Assert.False(friendService.IsItUserFriend(RandomValuesFactory.GetRandomInt(),
                    RandomValuesFactory.GetRandomInt()));
            }
        }
        [Fact]
        public void SendUserId_ListOfFriends()
        {
            int testUserId = RandomValuesFactory.GetRandomInt();
            int randomCount = RandomValuesFactory.GetRandomInt(2, 10);

            using (BirdyContext context = ContextFactory.GetContext())
            {

                for (int i = 0; i < randomCount; i++)
                {
                    int friendId = RandomValuesFactory.GetRandomInt();
                    context.Friends.Add(new Friend
                    {
                        FirstUserID = testUserId,
                        RequestAccepted = true,
                        SecondUserID = friendId
                    });
                    User user = DatabaseModelsFactory.GetRandomUser();
                    user.Id = friendId;
                    context.Users.Add(user);
                }

                context.SaveChanges();

                FriendService friendService = new FriendService(context);

                Assert.Equal(randomCount, friendService.GetFriends(testUserId).Count);
            }
        }

        [Fact]
        public void SendFriendRequest_InsertRequestToDb()
        {
            int firstUserId = RandomValuesFactory.GetRandomInt();
            int secondUserId = RandomValuesFactory.GetRandomInt();

            using (BirdyContext context = ContextFactory.GetContext())
            {
                FriendService friendService = new FriendService(context);

                friendService.SendFriendRequest(firstUserId, secondUserId);

                Assert.NotNull(context.Friends.SingleOrDefault(k =>
                    k.FirstUserID == firstUserId && k.SecondUserID == secondUserId && !k.RequestAccepted));
            }
        }

        [Fact]
        public void AcceptRequest_UpdateRequestDb()
        {
            int firstUserId = RandomValuesFactory.GetRandomInt();
            int secondUserId = RandomValuesFactory.GetRandomInt();

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.Friends.Add(new Friend
                {
                    FirstUserID = firstUserId,
                    SecondUserID = secondUserId,
                    RequestAccepted = false
                });
                context.SaveChanges();

                FriendService friendService = new FriendService(context);

                friendService.AcceptFriendRequest(firstUserId, secondUserId);

                Assert.NotNull(context.Friends.SingleOrDefault(k =>
                    k.RequestAccepted && k.FirstUserID == firstUserId && k.SecondUserID == secondUserId));
            }
        }

        [Fact]
        public void AcceptRequest_NullReferenceException()
        {
            using (BirdyContext context = ContextFactory.GetContext())
            {
                FriendService friendService = new FriendService(context);

                Assert.Throws<NullReferenceException>(() =>
                    friendService.AcceptFriendRequest(RandomValuesFactory.GetRandomInt(),
                        RandomValuesFactory.GetRandomInt()));
            }
        }

        [Fact]
        public void DeleteFriend_UpdateRequestDb()
        {
            int firstUserId = RandomValuesFactory.GetRandomInt();
            int secondUserId = RandomValuesFactory.GetRandomInt();

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.Friends.Add(new Friend
                {
                    FirstUserID = firstUserId,
                    SecondUserID = secondUserId,
                    RequestAccepted = true
                });
                context.SaveChanges();

                FriendService friendService = new FriendService(context);
                friendService.DeleteFriend(firstUserId, secondUserId);

                Assert.NotNull(context.Friends.SingleOrDefault(k =>
                    k.FirstUserID == firstUserId && k.SecondUserID == secondUserId && !k.RequestAccepted));
            }
        }

        [Fact]
        public void DeleteFriend_FlipRequestDb()
        {
            int firstUserId = RandomValuesFactory.GetRandomInt();
            int secondUserId = RandomValuesFactory.GetRandomInt();

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.Friends.Add(new Friend
                {
                    FirstUserID = firstUserId,
                    SecondUserID = secondUserId,
                    RequestAccepted = true
                });
                context.SaveChanges();

                FriendService friendService = new FriendService(context);
                friendService.DeleteFriend(secondUserId, firstUserId);

                Assert.NotNull(context.Friends.SingleOrDefault(k =>
                    k.FirstUserID == secondUserId && k.SecondUserID == firstUserId && !k.RequestAccepted));
            }
        }
    }
}
