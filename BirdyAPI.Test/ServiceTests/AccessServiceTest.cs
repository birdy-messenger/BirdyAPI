using System;
using System.Security.Authentication;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Services;
using BirdyAPI.Tools.Exceptions;
using BirdyAPI.Types;
using Xunit;

namespace BirdyAPI.Test.ServiceTests
{
    public class AccessServiceTest
    {
        private static readonly Random Random;

        static AccessServiceTest()
        {
            Random = new Random();
        }

        private AccessService GetAccessService()
        {
            return new AccessService(TestFactory.GetContext());
        }

        [Fact]
        public void SendInvalidToken_ArgumentException()
        {
            AccessService accessService = GetAccessService();

            Assert.Throws<AuthenticationException>(() => accessService.ValidateToken(Guid.NewGuid()));
        }
        [Fact]
        public void SendValidToken_UserId()
        {
            Guid randomToken = Guid.NewGuid();
            int randomUserId = Random.Next();

            BirdyContext context = TestFactory.GetContext();
            AccessService accessService = new AccessService(context);

            context.UserSessions.Add(new UserSession {Token = randomToken, UserId = randomUserId});
            context.SaveChanges();
            Assert.Equal(randomUserId, accessService.ValidateToken(randomToken));
        }

        [Fact]
        public void CheckInvalidUserRights_InsufficientRightsException()
        {
            AccessService accessService = GetAccessService();
            int randomUserId = Random.Next();
            int randomChatNumber = Random.Next();

            Assert.Throws<InsufficientRightsException>(() => accessService.CheckChatUserAccess(randomUserId, randomChatNumber, ChatStatus.User));
        }

        [Fact]
        public void CheckValidUserRights_Ok()
        {
            BirdyContext context = TestFactory.GetContext();

            Guid randomChatId = Guid.NewGuid();
            int randomChatNumber = Random.Next();
            int randomUserId = Random.Next();

            ChatUser currentChatUser = new ChatUser
            {
                ChatID = randomChatId,
                ChatNumber = randomChatNumber,
                UserInChatID = randomUserId,
                Status = ChatStatus.Admin
            };

            context.ChatUsers.Add(currentChatUser);
            context.SaveChanges();

            AccessService accessService = new AccessService(context);
            accessService.CheckChatUserAccess(currentChatUser.UserInChatID, currentChatUser.ChatNumber,
                ChatStatus.Admin);
        }
    }
}
