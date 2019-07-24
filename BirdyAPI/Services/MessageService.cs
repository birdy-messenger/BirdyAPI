using System;
using System.Linq;
using BirdyAPI.DataBaseModels;

namespace BirdyAPI.Services
{
    public class MessageService
    {
        private readonly BirdyContext _context;
        public MessageService(BirdyContext context)
        {
            _context = context;
        }

        public void SendMessageToUser(int currentUserId, int userId, string message)
        {
            ChatUser currentChat = _context.ChatUsers.Where(k => k.UserInChatID == currentUserId)
                .Intersect(_context.ChatUsers.Where(k => k.UserInChatID == userId)).SingleOrDefault();

            if (currentChat == null)
            {
                ChatUser firstUser = new ChatUser {ChatID = Guid.NewGuid(), UserInChatID = currentUserId};
                ChatUser secondUser = new ChatUser { ChatID = firstUser.ChatID, UserInChatID = userId };
                _context.ChatUsers.Add(firstUser);
                _context.ChatUsers.Add(secondUser);
                _context.SaveChanges();
            }

            //Message newMessage = new Message
              //{AuthorID = currentUserId, ConversationID = default, SendDate = DateTime.Now, Text = message};
        }
    }
}