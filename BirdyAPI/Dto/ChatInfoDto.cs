using System;

namespace BirdyAPI.Dto
{
    public class ChatInfoDto
    {
        public Guid ChatID { get; set; }
        public string ChatName { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastMessageTime { get; set; }
    }
}
