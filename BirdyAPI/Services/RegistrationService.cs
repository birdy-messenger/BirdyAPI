using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BirdyAPI.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BirdyAPI.Services
{
    public class RegistrationService
    {
        private readonly UserContext _context;

        public RegistrationService(UserContext context)
        {
            _context = context;
        }

        public string CreateNewAccount(User user)
        {
            if (_context.Users?.FirstOrDefault(k => k.Email == user.Email) != null)
                return JsonConvert.SerializeObject(new {ErrorMessage = "Duplicate account"});

            user.Token = new Random().Next(int.MaxValue / 2, int.MaxValue);
            user.CurrentStatus = UserStatus.Unconfirmed;

            _context.Add(user);
            _context.SaveChanges();

            string userReference = "birdytestapi.azurewebsites.net/api/confirmemail" +
                                   new QueryBuilder { { "id", user.Id.ToString() } }.ToQueryString();
            EmailConfirm(user.Email, userReference);

            return JsonConvert.SerializeObject(new { FirstName = user.FirstName});
        }

        private async void EmailConfirm(string email, string confirmReference)
        {
            SendGridClient client = new SendGridClient(apiKey: Configurations.SendGridAPIKey);
            
            EmailAddress birdyAddress = new EmailAddress(Configurations.OurEmailAddress, "Birdy");
            EmailAddress userAddress = new EmailAddress(email);

            string messageTopic = "Confirm your email";
            string HTMLmessage = Configurations.EmailConfirmMessage + $"<a href =\"https://{confirmReference}\">Confirm Link</a>";
            string plainTextContent = HTMLmessage; // Когда сообщение обрастет стилями и т.д. надо будет сделать нормально

            SendGridMessage message = MailHelper.CreateSingleEmail(birdyAddress, userAddress, messageTopic,
                plainTextContent, HTMLmessage);

            await client.SendEmailAsync(message);
        }
    }
}
