using System;
using System.ComponentModel.DataAnnotations;

namespace BirdyAPI.DataBaseModels
{
    public class ChatInfo
    {
        [Key]
        public Guid ChatID { get; set; }
        public string ChatName { get; set; }
        public ChatInfo() { }

        public ChatInfo(Guid chatId, string chatName)
        {
            ChatID = chatId;
            ChatName = chatName;
        }
    }
}
