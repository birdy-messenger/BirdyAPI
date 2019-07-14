using System;
using System.Configuration;
using System.Linq;
using System.Security.Authentication;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Tools;
using BirdyAPI.Types;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BirdyAPI.Services
{
    public class AppEntryService
    {
        private readonly BirdyContext _context;
        private readonly IConfiguration _configuration;
        public AppEntryService(BirdyContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public SimpleAnswerDto Authentication(AuthenticationDto user)
        {
            User currentUser = _context.Users.SingleOrDefault(k => k.Email == user.Email && k.PasswordHash == user.PasswordHash);

            if (currentUser != null)
            {
                if (currentUser.CurrentStatus == UserStatus.Unconfirmed)
                    throw new AuthenticationException();
                else if(currentUser.CurrentStatus == UserStatus.NeverUsed)
                {
                    throw new UnfinishedAccountException();
                }
                else
                {
                    UserSession currentSession = _context.UserSessions.Add(new UserSession{Token = Guid.NewGuid(), UserId = currentUser.Id}).Entity;
                    _context.SaveChanges();
                    return new SimpleAnswerDto{Result = currentSession.Token.ToString()};
                }
            }

            throw new ArgumentException();
        }



        public void GetUserConfirmed(string email, Guid token)
        {
            ConfirmToken currentConfirmToken = _context.ConfirmTokens.Find(email);
            if(currentConfirmToken == null || currentConfirmToken.Token != token)
                throw new ArgumentException();


            if (currentConfirmToken.TokenDate.AddDays(1) < DateTime.Now)
            {
                _context.ConfirmTokens.Remove(currentConfirmToken);
                _context.SaveChanges();
                throw new TimeoutException();
            }


            User confirmedUser = _context.Users.Single(k => k.Email == email);
            confirmedUser.CurrentStatus = UserStatus.NeverUsed;
            _context.Users.Update(confirmedUser);
            _context.ConfirmTokens.Remove(currentConfirmToken);
            _context.SaveChanges();
        }

        public void CreateNewAccount(RegistrationDto registrationData)
        {
            if (_context.Users.SingleOrDefault(k => k.Email == registrationData.Email) != null)
                throw new DuplicateAccountException();

            AddNewUser(registrationData);
            Guid currentToken = CreateConfirmToken(registrationData.Email);

            string userReference = "birdytestapi.azurewebsites.net/app/confirm" +
                                   new QueryBuilder { { "email", registrationData.Email }, {"token", currentToken.ToString()} }.ToQueryString();

            SendConfirmEmail(registrationData.Email, userReference);
        }

        private void AddNewUser(RegistrationDto registrationData)
        {
            User newUser = new User
            {
                Email = registrationData.Email,
                PasswordHash = registrationData.PasswordHash,
                FirstName = registrationData.FirstName,
                UniqueTag = null,
                RegistrationDate = DateTime.Now,
                CurrentStatus = UserStatus.Unconfirmed,
                AvatarReference = null
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        private Guid CreateConfirmToken(string email)
        {
            ConfirmToken newConfirmToken = new ConfirmToken
            {
                Email = email,
                Token = Guid.NewGuid(),
                TokenDate = DateTime.Now
            };
            _context.ConfirmTokens.Add(newConfirmToken);
            _context.SaveChanges();

            return newConfirmToken.Token;
        }
        public void ChangePassword(int id, ChangePasswordDto passwordChanges)
        {
            User currentUser = _context.Users.Find(id);

            if (currentUser.PasswordHash == passwordChanges.OldPassorwdHash)
            {
                currentUser.PasswordHash = passwordChanges.NewPasswordHash;
                _context.Users.Update(currentUser);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public void TerminateSession(Guid token, int userId)
        {
            UserSession currentSession = new UserSession{Token = token, UserId = userId};
            _context.UserSessions.Remove(currentSession);
            _context.SaveChanges();
        }

        public void TerminateSession(int userId)
        {
            foreach (var session in _context.UserSessions.Where(k => k.UserId == userId))
            {
                TerminateSession(session.Token, session.UserId);
            }
        }

        private async void SendConfirmEmail(string email, string confirmReference)
        {
            SendGridClient client = new SendGridClient(apiKey: ConfigurationManager.AppSettings["SendGrid"]);
            SendGridMessage message = MessageBuilder(email, confirmReference);

            await client.SendEmailAsync(message);
        }

        private SendGridMessage MessageBuilder(string email, string confirmReference)
        {

            EmailAddress birdyAddress = new EmailAddress(Configurations.OurEmailAddress, "Birdy");
            EmailAddress userAddress = new EmailAddress(email);

            string messageTopic = "Confirm your email";
            string HTMLmessage = Configurations.EmailConfirmMessage + $"<a href =\"https://{confirmReference}\">Confirm Link</a>";
            string plainTextContent = HTMLmessage; // Когда сообщение обрастет стилями и т.д. надо будет сделать нормально
            return MailHelper.CreateSingleEmail(birdyAddress, userAddress, messageTopic,
                plainTextContent, HTMLmessage);
        }
    }
}
