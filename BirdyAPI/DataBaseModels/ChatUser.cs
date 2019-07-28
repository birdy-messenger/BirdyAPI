using System;
using System.Runtime.InteropServices;
using BirdyAPI.Types;

namespace BirdyAPI.DataBaseModels
{
    public class ChatUser
    {
        public Guid ChatID { get; set; }
        public int UserInChatID { get; set; }
        public int ChatNumber { get; set; }
        public ChatStatus Status { get; set; }
        public ChatUser() { }

        private ChatUser(Guid chatId, int userInChatId, int chatNumber, ChatStatus userStatus)
        {
            ChatID = chatId;
            UserInChatID = userInChatId;
            ChatNumber = chatNumber;
            Status = userStatus;
        }

        public static ChatUser Create(int creatorId, int newChatNumber)
        {
            return new ChatUser(Guid.NewGuid(), creatorId, newChatNumber, ChatStatus.Admin);
        }
        public static ChatUser Create(Guid chatId, int userId, int newChatNumber)
        {
            return new ChatUser(chatId, userId, newChatNumber, ChatStatus.User);
        }
    }
}
