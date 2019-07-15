using System;

namespace BirdyAPI.DataBaseModels
{
    public class ChatUser
    {
        public Guid ChatID { get; set; }
        public int UserInChatID { get; set; }
        public int ChatNumber { get; set; }
    }
}
