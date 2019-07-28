using System;
using BirdyAPI.DataBaseModels;

namespace BirdyAPI.Dto
{
    public class DialogPreviewDto
    {
        public string InterlocutorUniqueTag { get; set; }
        public string LastMessage { get; set; }
        public string LastMessageAuthor { get; set; }
        public DateTime LastMessageTime { get; set; }
        public DialogPreviewDto() { }

        private DialogPreviewDto(string interlocutorUniqueTag, string lastMessage, string lastMessageAuthor, DateTime lastMessageTime)
        {
            InterlocutorUniqueTag = interlocutorUniqueTag;
            LastMessage = lastMessage;
            LastMessageAuthor = lastMessageAuthor;
            LastMessageTime = lastMessageTime;
        }
        public static DialogPreviewDto Create(string interlocutorUniqueTag, string lastMessageAuthor, Message dialogLastMessage)
        {
            string lastMessage = dialogLastMessage?.Text;
            DateTime lastMessageTime = dialogLastMessage?.SendDate ?? DateTime.MinValue;

            return new DialogPreviewDto(interlocutorUniqueTag, lastMessage, lastMessageAuthor, lastMessageTime);
        }
    }
}
