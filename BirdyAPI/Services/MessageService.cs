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
            DialogUser currentDialog = _context.DialogUsers.SingleOrDefault(k =>
                                           (k.FirstUserID == currentUserId && k.SecondUserID == userId) ||
                                           (k.FirstUserID == userId && k.SecondUserID == currentUserId)) ??
                                       InitNewDialog(currentUserId, userId);

            Message currentMessage = new Message
            {
                AuthorID = currentUserId,
                ConversationID = currentDialog.DialogID,
                MessageID = Guid.NewGuid(),
                SendDate = DateTime.Now,
                Text = message
            };

            _context.Messages.Add(currentMessage);
            _context.SaveChanges();
        }

        public void SendMessageToChat(int currentUserId, Guid chatId, string message)
        {
            Message currentMessage = new Message
             {
                 AuthorID = currentUserId,
                 ConversationID = chatId,
                 MessageID = Guid.NewGuid(),
                 SendDate = DateTime.Now,
                 Text = message
             };

             _context.Messages.Add(currentMessage);
             _context.SaveChanges();
        }

        private DialogUser InitNewDialog(int firstUserId, int secondUserId)
        {
            DialogUser newDialog = new DialogUser
            {
                DialogID = Guid.NewGuid(),
                FirstUserID = firstUserId,
                SecondUserID = secondUserId
            };
            _context.DialogUsers.Add(newDialog);
            _context.SaveChanges();

            return newDialog;
        }
    }
}