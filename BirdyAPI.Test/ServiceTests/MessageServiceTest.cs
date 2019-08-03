using System;
using System.Linq;
using BirdyAPI.Services;
using BirdyAPI.Test.Factories;
using Xunit;

namespace BirdyAPI.Test.ServiceTests
{
    public class MessageServiceTest
    {
        private static string RandomString => RandomValuesFactory.GetRandomString();
        private static int RandomUserId => RandomValuesFactory.GetRandomInt();
        private static Guid RandomConversationId => RandomValuesFactory.GetRandomGuid();
        private static BirdyContext Context => ContextFactory.GetContext();

        [Fact]
        public void SendMessageToChat_Ok()
        {
            Guid conversationId = RandomConversationId;
            int user = RandomUserId;
            string message = RandomString;

            BirdyContext context = Context;
            MessageService messageService = new MessageService(context);

            messageService.SendMessageToChat(user, conversationId, message);
            var insertedValue = context.Messages.SingleOrDefault(k => k.Text == message && k.ConversationID == conversationId && k.AuthorID == user);

            Assert.NotNull(insertedValue);
        }

        [Fact]
        public void SendMessageToUser_Ok()
        {
            int userId = RandomUserId;
            int secondUserId = RandomUserId;
            string message = RandomString;

            BirdyContext context = Context;
            MessageService messageService = new MessageService(context);

            messageService.SendMessageToUser(userId, secondUserId, message);
            var sentMessage = context.Messages.SingleOrDefault(k => k.Text == message && k.AuthorID == userId);

            Assert.NotNull(sentMessage);
        }
    }
}
