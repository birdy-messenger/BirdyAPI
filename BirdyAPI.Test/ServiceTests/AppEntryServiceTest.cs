using System;
using System.Linq;
using System.Security.Authentication;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Services;
using BirdyAPI.Test.Factories;
using BirdyAPI.Tools.Exceptions;
using BirdyAPI.Types;
using Xunit;

namespace BirdyAPI.Test.ServiceTests
{
    public class AppEntryServiceTest
    {
        [Fact]
        public void RegistrationWithRepeatingEmail_DuplicateAccountException()
        {
            RegistrationDto regData = RandomValuesFactory.GetRandomRegistrationData();
            User createdUser = DatabaseModelsFactory.GetRandomUser();
            createdUser.Email = regData.Email;
            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.Users.Add(createdUser);
                context.SaveChanges();

                AppEntryService appEntryService = new AppEntryService(context);
                Assert.Throws<DuplicateAccountException>(() => appEntryService.CreateNewAccount(regData));
            }
        }

        [Fact]
        public void RegistrationWithCorrectEmail_NewUserInDatabase()
        {
            RegistrationDto regData = RandomValuesFactory.GetRandomRegistrationData();
            using (BirdyContext context = ContextFactory.GetContext())
            {
                AppEntryService appEntryService = new AppEntryService(context);
                appEntryService.CreateNewAccount(regData);
                Assert.Equal(UserStatus.Unconfirmed,
                    context.Users.SingleOrDefault(k => k.Email == regData.Email)?.CurrentStatus);
            }
        }

        [Fact]
        public void ConfirmEmail_UpdateUserStatus()
        {
            User user = DatabaseModelsFactory.GetRandomUser();
            ConfirmToken confirmToken = ConfirmToken.Create(user.Email);
            user.CurrentStatus = UserStatus.Unconfirmed;

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.Users.Add(user);
                context.ConfirmTokens.Add(confirmToken);
                context.SaveChanges();

                AppEntryService appEntryService = new AppEntryService(context);
                appEntryService.GetUserConfirmed(user.Email, confirmToken.Token);
                Assert.Equal(UserStatus.Confirmed, context.Users.Find(user.Id).CurrentStatus);
            }
        }

        [Fact]
        public void ConfirmEmailTimeoutToken_TimeoutException()
        {
            string randomEmail = RandomValuesFactory.GetRandomString();
            ConfirmToken confirmToken = ConfirmToken.Create(randomEmail);
            confirmToken.TokenDate = confirmToken.TokenDate.AddDays(-2);

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.ConfirmTokens.Add(confirmToken);
                context.SaveChanges();

                AppEntryService appEntryService = new AppEntryService(context);
                Assert.Throws<TimeoutException>(() =>
                    appEntryService.GetUserConfirmed(randomEmail, confirmToken.Token));
            }
        }

        [Fact]
        public void TerminateSession_DeleteSessionFromDatabase()
        {
            UserSession userSession = DatabaseModelsFactory.GetRandomUserSession();

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.UserSessions.Add(userSession);
                context.SaveChanges();

                AppEntryService appEntryService = new AppEntryService(context);
                appEntryService.TerminateSession(userSession.Token, userSession.UserId);

                Assert.Null(context.UserSessions.SingleOrDefault(k =>
                    k.Token == userSession.Token && k.UserId == userSession.UserId));
            }
        }

        [Fact]
        public void TerminateAllSession_ClearSessionsFromDatabase()
        {
            UserSession firstUserSession = DatabaseModelsFactory.GetRandomUserSession();
            UserSession secondUserSession = DatabaseModelsFactory.GetRandomUserSession();
            secondUserSession.UserId = firstUserSession.UserId;

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.UserSessions.AddRange(firstUserSession, secondUserSession);
                context.SaveChanges();

                AppEntryService appEntryService = new AppEntryService(context);
                appEntryService.TerminateAllSessions(firstUserSession.UserId);

                Assert.Null(context.UserSessions.SingleOrDefault(k =>
                    k.UserId == firstUserSession.UserId &&
                    (k.Token == firstUserSession.Token || k.Token == secondUserSession.Token)));
            }
        }

        [Fact]
        public void ChangePassword_UpdatePassword()
        {
            User user = DatabaseModelsFactory.GetRandomUser();
            ChangePasswordDto passwordsData = new ChangePasswordDto
            {
                NewPasswordHash = RandomValuesFactory.GetRandomString(),
                OldPasswordHash = user.PasswordHash
            };

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.Users.Add(user);
                context.SaveChanges();

                AppEntryService appEntryService = new AppEntryService(context);
                appEntryService.ChangePassword(user.Id, passwordsData);
                Assert.Equal(passwordsData.NewPasswordHash, context.Users.Find(user.Id).PasswordHash);
            }
        }

        [Fact]
        public void ChangePasswordWithWrongOldPassword_ArgumentException()
        {
            User user = DatabaseModelsFactory.GetRandomUser();
            ChangePasswordDto passwordsData = new ChangePasswordDto
            {
                NewPasswordHash = RandomValuesFactory.GetRandomString(),
                OldPasswordHash = RandomValuesFactory.GetRandomString()
            };

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.Users.Add(user);
                context.SaveChanges();

                AppEntryService appEntryService = new AppEntryService(context);
                Assert.Throws<ArgumentException>(() => appEntryService.ChangePassword(user.Id, passwordsData));
            }
        }

        [Fact]
        public void AuthInvalidData_ArgumentException()
        {
            User user = DatabaseModelsFactory.GetRandomUser();

            AuthenticationDto authData = new AuthenticationDto
                {
                    Email = RandomValuesFactory.GetRandomString(),
                    PasswordHash = RandomValuesFactory.GetRandomString()
                };

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.Users.Add(user);
                context.SaveChanges();

                AppEntryService appEntryService = new AppEntryService(context);
                Assert.Throws<ArgumentException>(() => appEntryService.Authentication(authData));
            }
        }

        [Fact]
        public void AuthUnconfirmedAccount_AuthenticationException()
        {
            User user = DatabaseModelsFactory.GetRandomUser();
            user.CurrentStatus = UserStatus.Unconfirmed;

            AuthenticationDto authData = new AuthenticationDto
            {
                Email = user.Email,
                PasswordHash = user.PasswordHash
            };

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.Users.Add(user);
                context.SaveChanges();

                AppEntryService appEntryService = new AppEntryService(context);
                Assert.Throws<AuthenticationException>(() => appEntryService.Authentication(authData));
            }
        }

        [Fact]
        public void Auth_UserSession()
        {
            User user = DatabaseModelsFactory.GetRandomUser();
            user.CurrentStatus = UserStatus.Confirmed;

            AuthenticationDto authData = new AuthenticationDto
            {
                Email = user.Email,
                PasswordHash = user.PasswordHash
            };

            using (BirdyContext context = ContextFactory.GetContext())
            {
                context.Users.Add(user);
                context.SaveChanges();

                AppEntryService appEntryService = new AppEntryService(context);
                SimpleAnswerDto answer = appEntryService.Authentication(authData);
                Assert.NotNull(context.UserSessions.Find(Guid.Parse(answer.Result)));
            }
        }
    }
}