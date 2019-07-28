using System;

namespace BirdyAPI.DataBaseModels
{
    public class Message
    {
        public Guid ConversationID { get; set; }
        public Guid MessageID { get; set; }
        public int AuthorID { get; set; }
        public DateTime SendDate { get; set; }
        public string Text { get; set; }
        public Message() { }

        private Message(Guid conversationId, Guid messageId, int authorId, DateTime messageTime, string message)
        {
            ConversationID = conversationId;
            MessageID = messageId;
            AuthorID = authorId;
            SendDate = messageTime;
            Text = message;
        }

        public static Message Create(int authorId, Guid conversationId, string message)
        {
            return new Message(conversationId, Guid.NewGuid(), authorId, DateTime.Now, message);
        }
    }
}
