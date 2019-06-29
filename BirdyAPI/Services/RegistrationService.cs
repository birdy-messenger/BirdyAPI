using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BirdyAPI.DataBaseModels;
using BirdyAPI.Dto;
using BirdyAPI.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
    
namespace BirdyAPI.Services
{
    public class RegistrationService
    {
        private readonly UserContext _context;
        public IConfiguration Configuration;

        public RegistrationService(UserContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
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
            SendGridClient client = new SendGridClient(apiKey: Configuration.GetConnectionString("SendGrid"));
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
