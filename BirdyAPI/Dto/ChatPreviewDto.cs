using System;
using BirdyAPI.DataBaseModels;

namespace BirdyAPI.Dto
{
    public class ChatPreviewDto
    {
        public int ChatNumber { get; set; }
        public string ChatName { get; set; }
        public string LastMessage { get; set; }
        public string LastMessageAuthor { get; set; }
        public DateTime LastMessageTime { get; set; }

        public ChatPreviewDto() { }
        private ChatPreviewDto(int chatNumber, string chatName, string lastMessage, string lastMessageAuthor, DateTime lastMessageTime)
        {
            ChatNumber = chatNumber;
            ChatName = chatName;
            LastMessage = lastMessage;
            LastMessageAuthor = lastMessageAuthor;
            LastMessageTime = lastMessageTime;
        }

        public static ChatPreviewDto Create(int chatNumber, string chatName, string lastMessageAuthor, Message chatLastMessage)
        {
            string lastMessage = chatLastMessage?.Text;
            DateTime lastMessageTime = chatLastMessage?.SendDate ?? DateTime.MinValue;

            return new ChatPreviewDto(chatNumber, chatName, lastMessage, lastMessageAuthor, lastMessageTime);
        }
    }
}
