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
            DialogUser currentDialog = _context.DialogUsers
                                           .SingleOrDefault(k =>
                                               (k.FirstUserID == currentUserId && k.SecondUserID == userId) ||
                                               (k.FirstUserID == userId && k.SecondUserID == currentUserId)) ??
                                       InitNewDialog(currentUserId, userId);

            Message currentMessage = Message.Create(currentUserId, currentDialog.DialogID, message);
            _context.Messages.Add(currentMessage);
            _context.SaveChanges();
        }

        public void SendMessageToChat(int currentUserId, Guid chatId, string message)
        {
            Message currentMessage = Message.Create(currentUserId, chatId, message);
            _context.Messages.Add(currentMessage);
             _context.SaveChanges();
        }

        private DialogUser InitNewDialog(int firstUserId, int secondUserId)
        {
            DialogUser newDialog = DialogUser.Create(firstUserId, secondUserId);
            _context.DialogUsers.Add(newDialog);
            _context.SaveChanges();

            return newDialog;
        }
    }
}