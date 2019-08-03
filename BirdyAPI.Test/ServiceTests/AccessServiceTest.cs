using System;
using System.Security.Authentication;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Services;
using BirdyAPI.Test.Factories;
using BirdyAPI.Tools.Exceptions;
using BirdyAPI.Types;
using Xunit;

namespace BirdyAPI.Test.ServiceTests
{
    public class AccessServiceTest
    {
        private static int RandomUserId => RandomValuesFactory.GetRandomInt();
        private static int RandomChatNumber => RandomValuesFactory.GetRandomInt();
        private static BirdyContext Context => ContextFactory.GetContext();
        private static Guid RandomToken => RandomValuesFactory.GetRandomGuid();
        private static Guid RandomChatId => RandomValuesFactory.GetRandomGuid();

        private AccessService GetAccessService()
        {
            return new AccessService(Context);
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
            UserSession testSession = new UserSession {Token = RandomToken, UserId = RandomUserId};

            BirdyContext context = Context;
            context.UserSessions.Add(testSession);
            context.SaveChanges();

            AccessService accessService = new AccessService(context);

            Assert.Equal(testSession.UserId, accessService.ValidateToken(testSession.Token));
        }

        [Fact]
        public void CheckInvalidUserRights_InsufficientRightsException()
        {
            AccessService accessService = GetAccessService();

            Assert.Throws<InsufficientRightsException>(() => accessService.CheckChatUserAccess(RandomUserId, RandomChatNumber, ChatStatus.User));
        }

        [Fact]
        public void CheckValidUserRights_Ok()
        {

            ChatUser currentChatUser = new ChatUser
            {
                ChatID = RandomChatId,
                ChatNumber = RandomChatNumber,
                UserInChatID = RandomUserId,
                Status = ChatStatus.Admin
            };

            BirdyContext context = Context;
            context.ChatUsers.Add(currentChatUser);
            context.SaveChanges();

            AccessService accessService = new AccessService(context);
            accessService.CheckChatUserAccess(currentChatUser.UserInChatID, currentChatUser.ChatNumber,
                ChatStatus.Admin);
        }
    }
}
