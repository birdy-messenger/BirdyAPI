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
            using (BirdyContext context = ContextFactory.GetContext())
            {
                AccessService accessService = new AccessService(context);

                Assert.Throws<AuthenticationException>(() => accessService.ValidateToken(Guid.NewGuid()));
            }
        }
        [Fact]
        public void SendValidToken_UserId()
        {
            UserSession testSession = DatabaseModelsFactory.GetRandomUserSession();

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.UserSessions.Add(testSession);
                context.SaveChanges();

                AccessService accessService = new AccessService(context);

                Assert.Equal(testSession.UserId, accessService.ValidateToken(testSession.Token));
            }
        }

        [Fact]
        public void CheckInvalidUserRights_InsufficientRightsException()
        {
            using (BirdyContext context = ContextFactory.GetContext())
            {
                AccessService accessService = new AccessService(context);

                Assert.Throws<InsufficientRightsException>(() =>
                    accessService.CheckChatUserAccess(RandomValuesFactory.GetRandomGuid(),
                        RandomValuesFactory.GetRandomInt(), ChatStatus.User));
            }
        }

        [Fact]
        public void CheckValidUserRights_Ok()
        {

            ChatUser currentChatUser = DatabaseModelsFactory.GetRandomChatUserAdmin();

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.ChatUsers.Add(currentChatUser);
                context.SaveChanges();

                AccessService accessService = new AccessService(context);
                accessService.CheckChatUserAccess(currentChatUser.ChatID, currentChatUser.UserInChatID,
                    ChatStatus.Admin);
            }
        }
    }
}
