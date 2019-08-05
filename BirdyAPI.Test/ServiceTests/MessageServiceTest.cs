using System;
using System.Linq;
using BirdyAPI.Services;
using BirdyAPI.Test.Factories;
using Xunit;

namespace BirdyAPI.Test.ServiceTests
{
    public class MessageServiceTest
    {
        [Fact]
        public void SendMessageToChat_Ok()
        {
            Guid conversationId = RandomValuesFactory.GetRandomGuid();
            int user = RandomValuesFactory.GetRandomInt();
            string message = RandomValuesFactory.GetRandomString();

            using (BirdyContext context = ContextFactory.GetContext())
            {
                MessageService messageService = new MessageService(context);

                messageService.SendMessageToChat(user, conversationId, message);

                Assert.NotNull(context.Messages.SingleOrDefault(k =>
                    k.Text == message && k.ConversationID == conversationId && k.AuthorID == user));
            }
        }

        [Fact]
        public void SendMessageToUser_Ok()
        {
            int userId = RandomValuesFactory.GetRandomInt();
            int secondUserId = RandomValuesFactory.GetRandomInt();
            string message = RandomValuesFactory.GetRandomString();

            using (BirdyContext context = ContextFactory.GetContext())
            {
                MessageService messageService = new MessageService(context);

                messageService.SendMessageToUser(userId, secondUserId, message);

                Assert.NotNull(context.Messages.SingleOrDefault(k => k.Text == message && k.AuthorID == userId));
            }
        }
    }
}
