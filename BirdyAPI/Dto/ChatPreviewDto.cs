using System;

namespace BirdyAPI.Dto
{
    public class ChatPreviewDto
    {
        public int ChatNumber { get; set; }
        public string ChatName { get; set; }
        public string LastMessage { get; set; }
        public string LastMessageAuthor { get; set; }
        public DateTime LastMessageTime { get; set; }
    }
}
