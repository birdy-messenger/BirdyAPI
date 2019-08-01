using System;
using System.Security.Authentication;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Services;
using Microsoft.AspNetCore.DataProtection;
using Xunit;

namespace BirdyAPI.Test
{
    public class AccessServiceTest
    {
        private static readonly Random Random;

        static AccessServiceTest()
        {
            Random = new Random();
        }

        [Fact]
        public void SendInvalidToken_ArgumentException()
        {
            AccessService accessService = new AccessService(TestInit.GetContext());
            Assert.Throws<AuthenticationException>(() => accessService.ValidateToken(Guid.NewGuid()));
        }
        [Fact]
        public void SendValidToken_UserId()
        {
            Guid testToken = Guid.NewGuid();
            int testUserId = Random.Next();
            BirdyContext context = TestInit.GetContext();
            AccessService accessService = new AccessService(context);
            context.UserSessions.Add(new UserSession {Token = testToken, UserId = testUserId});
            Assert.Equal(testUserId, accessService.ValidateToken(testToken));
        }
    }
}
