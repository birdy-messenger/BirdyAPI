using System;
using System.Linq;
using System.Security.Authentication;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Models;
using BirdyAPI.Tools;
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
                else
                {
                    UserSession currentSession = _context.UserSessions.Add(new UserSession(currentUser.Id)).Entity;
                    _context.SaveChanges();
                    return new SimpleAnswerDto{Result = currentSession.Token.ToString()};
                }
            }

            throw new ArgumentException("Invalid email or password");
        }



        public string GetUserConfirmed(int id)
        {
            User user = _context.Users.Find(id);
            if (user == null)
                throw new ArgumentException("Invalid link");

            user.CurrentStatus = UserStatus.Confirmed;

            _context.Users.Update(user);
            _context.SaveChanges();
            return JsonConvert.SerializeObject(new { Status = user.CurrentStatus });
            //Здесь вообще должно быть что-то другое, пока оставлю так
        }

        public void CreateNewAccount(RegistrationDto registrationData)
        {
            if (_context.Users.SingleOrDefault(k => k.Email == registrationData.Email) != null)
                throw new DuplicateAccountException("Duplicate account");

            User newUser = new User
            {
                Email = registrationData.Email,
                PasswordHash = registrationData.PasswordHash,
                FirstName = registrationData.FirstName,
                UniqueTag = registrationData.UniqueTag,
                RegistrationDate = DateTime.Now,
                CurrentStatus = UserStatus.Unconfirmed
        };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            string userReference = "birdytestapi.azurewebsites.net/app/confirm" +
                                   new QueryBuilder { { "id", newUser.Id.ToString() } }.ToQueryString();

            SendConfirmEmail(newUser.Email, userReference);
        }

        public void ChangePassword(int id, ChangePasswordDto passwordChanges)
        {
            User currentUser = _context.Users.Find(id);
            if (currentUser == null)
            {
                throw new Exception("User doesn't exist");
            }
            if (currentUser.PasswordHash == passwordChanges.OldPassorwdHash)
            {
                currentUser.PasswordHash = passwordChanges.NewPasswordHash;
                _context.Users.Update(currentUser);
                return;
            }
            else
            {
                throw new ArgumentException("Wrong password");
            }

        }

        public void ExitApp(Guid token, int userId)
        {
            UserSession currentSession = new UserSession{Token = token, UserId = userId};
            _context.UserSessions.Remove(currentSession);
            _context.SaveChanges();
        }

        public void FullExitApp(Guid token, int userId)
        {
            UserSession currentSession = new UserSession{Token = token, UserId = userId};
            _context.UserSessions.RemoveRange(_context.UserSessions.Where(k => k.UserId == currentSession.UserId));
            _context.SaveChanges();
        }

        private async void SendConfirmEmail(string email, string confirmReference)
        {
            SendGridClient client = new SendGridClient(apiKey: _configuration.GetConnectionString("SendGrid"));
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
