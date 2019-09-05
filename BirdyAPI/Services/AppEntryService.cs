using System;
using System.Linq;
using System.Security.Authentication;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Tools.Exceptions;
using BirdyAPI.Types;
using Microsoft.AspNetCore.Http.Extensions;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BirdyAPI.Services
{
    public class AppEntryService
    {
        private readonly BirdyContext _context;

        public AppEntryService(BirdyContext context)
        {
            _context = context;
        }

        public SimpleAnswerDto Authentication(AuthenticationDto user)
        {
            User currentUser =
                _context.Users.SingleOrDefault(k => k.Email == user.Email && k.PasswordHash == user.PasswordHash);

            if (currentUser == null)
                throw new ArgumentException("Wrong password or email");

            if (currentUser.CurrentStatus == UserStatus.Unconfirmed)
                throw new AuthenticationException("User need to confirm email");

            UserSession currentSession = _context.UserSessions
                .Add(UserSession.Create(currentUser.Id)).Entity; 
            _context.SaveChanges();

            return new SimpleAnswerDto {Result = currentSession.Token.ToString()};
        }

        public void GetUserConfirmed(string email, Guid token)
        {
            ConfirmToken currentConfirmToken = _context.ConfirmTokens.Find(email);

            if(currentConfirmToken == null || currentConfirmToken.Token != token)
                throw new ArgumentException("Wrong email");


            if (currentConfirmToken.TokenDate.AddDays(1) < DateTime.Now)
            {
                _context.ConfirmTokens.Remove(currentConfirmToken);
                _context.SaveChanges();
                throw new TimeoutException("Token expired");
            }


            User confirmedUser = _context.Users.Single(k => k.Email == email);
            confirmedUser.CurrentStatus = UserStatus.Confirmed;
            _context.Users.Update(confirmedUser);
            _context.ConfirmTokens.Remove(currentConfirmToken);
            _context.SaveChanges();
        }

        public void CreateNewAccount(RegistrationDto registrationData)
        {
            if (_context.Users.SingleOrDefault(k => k.Email == registrationData.Email) != null)
                throw new DuplicateAccountException("Birdy messenger contains account with that email");

            AddNewUser(registrationData);
            Guid currentToken = CreateConfirmToken(registrationData.Email);

            string userReference = "birdyapi.azurewebsites.net/app/confirm" +
                                   new QueryBuilder { { "email", registrationData.Email }, {"token", currentToken.ToString()} }.ToQueryString();

            SendConfirmEmail(registrationData.Email, userReference);
        }

        private void AddNewUser(RegistrationDto registrationData)
        {
            User newUser = User.Create(registrationData);

            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        private Guid CreateConfirmToken(string email)
        {
            ConfirmToken newConfirmToken = ConfirmToken.Create(email);
            _context.ConfirmTokens.Add(newConfirmToken);
            _context.SaveChanges();

            return newConfirmToken.Token;
        }
        public void ChangePassword(int id, ChangePasswordDto passwordChanges)
        {
            User currentUser = _context.Users.Find(id);

            if (currentUser.PasswordHash == passwordChanges.OldPasswordHash)
            {
                currentUser.PasswordHash = passwordChanges.NewPasswordHash;
                _context.Users.Update(currentUser);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Wrong Password");
            }
        }

        public void TerminateSession(Guid token, int userId)
        {
             UserSession currentSession = _context.UserSessions.Single(k => k.Token == token && k.UserId == userId);
            _context.UserSessions.Remove(currentSession);
            _context.SaveChanges();
        }

        public void TerminateAllSessions(int userId)
        {
            foreach (var session in _context.UserSessions.Where(k => k.UserId == userId))
            {
                TerminateSession(session.Token, session.UserId);
            }
        }

        private async void SendConfirmEmail(string email, string confirmReference)
        {
            SendGridClient client = new SendGridClient(Configurations.SendGridApiKey);
            SendGridMessage message = MessageBuilder(email, confirmReference);

            await client.SendEmailAsync(message);
        }

        private SendGridMessage MessageBuilder(string email, string confirmReference)
        {

            EmailAddress birdyAddress = new EmailAddress(Configurations.OurEmailAddress, "Birdy");
            EmailAddress userAddress = new EmailAddress(email);

            string messageTopic = "Confirm your email";
            string htmlMessage = Configurations.EmailConfirmMessage + $"<a href =\"https://{confirmReference}\">Confirm Link</a>";
            string plainTextContent = htmlMessage; // Когда сообщение обрастет стилями и т.д. надо будет сделать нормально
            return MailHelper.CreateSingleEmail(birdyAddress, userAddress, messageTopic,
                plainTextContent, htmlMessage);
        }
    }
}
