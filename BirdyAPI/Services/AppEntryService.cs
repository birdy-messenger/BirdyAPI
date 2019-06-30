using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BirdyAPI.Services
{
    public class AppEntryService
    {
        private readonly UserContext _context;
        private readonly IConfiguration _configuration;
        public AppEntryService(UserContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public UserSessionDto Authentication(AuthenticationDto user)
        {
            User currentUser = _context.Users.SingleOrDefault(k => k.Email == user.Email && k.PasswordHash == user.PasswordHash);
            if (currentUser != null)
            {
                if (currentUser.CurrentStatus == UserStatus.Unconfirmed)
                    throw new Exception("Need to confirm email");
                else
                    return new UserSessionDto(currentUser.Id, currentUser.Token);
            }

            throw new ArgumentException("Invalid email or password");
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
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
            //Здесь вообще должна быть html страничка, пока оставлю так
        }

        public string CreateNewAccount(RegistrationDto registrationData)
        {
            if (_context.Users?.FirstOrDefault(k => k.Email == registrationData.Email) != null)
                throw new ArgumentException("Duplicate account");

            User newUser = new User(registrationData);

            _context.Add(newUser);
            _context.SaveChanges();

            string userReference = "birdytestapi.azurewebsites.net/api/confirmemail" +
                                   new QueryBuilder { { "id", newUser.Id.ToString() } }.ToQueryString();

            SendConfirmEmail(newUser.Email, userReference);

            return "Message sent";
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
