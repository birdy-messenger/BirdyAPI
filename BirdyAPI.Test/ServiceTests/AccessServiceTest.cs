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
        [Fact]
        public void SendInvalidToken_ArgumentException()
        {
            AccessService accessService = new AccessService(ContextFactory.GetContext());

            Assert.Throws<AuthenticationException>(() => accessService.ValidateToken(Guid.NewGuid()));
        }
        [Fact]
        public void SendValidToken_UserId()
        {
            UserSession testSession = new UserSession
                {Token = RandomValuesFactory.GetRandomGuid(), UserId = RandomValuesFactory.GetRandomInt()};

            BirdyContext context = ContextFactory.GetContext();
            context.UserSessions.Add(testSession);
            context.SaveChanges();

            AccessService accessService = new AccessService(context);

            Assert.Equal(testSession.UserId, accessService.ValidateToken(testSession.Token));
        }

        [Fact]
        public void CheckInvalidUserRights_InsufficientRightsException()
        {
            AccessService accessService = new AccessService(ContextFactory.GetContext());

            Assert.Throws<InsufficientRightsException>(() =>
                accessService.CheckChatUserAccess(RandomValuesFactory.GetRandomInt(),
                    RandomValuesFactory.GetRandomInt(), ChatStatus.User));
        }

        [Fact]
        public void CheckValidUserRights_Ok()
        {

            ChatUser currentChatUser = new ChatUser
            {
                ChatID = RandomValuesFactory.GetRandomGuid(),
                ChatNumber = RandomValuesFactory.GetRandomInt(),
                UserInChatID = RandomValuesFactory.GetRandomInt(),
                Status = ChatStatus.Admin
            };

            BirdyContext context = ContextFactory.GetContext();
            context.ChatUsers.Add(currentChatUser);
            context.SaveChanges();

            AccessService accessService = new AccessService(context);
            accessService.CheckChatUserAccess(currentChatUser.UserInChatID, currentChatUser.ChatNumber,
                ChatStatus.Admin);
        }
    }
}
