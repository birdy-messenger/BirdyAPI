using System;

namespace BirdyAPI.Dto
{
    public class DialogPreviewDto
    {
        public string InterlocutorUniqueTag { get; set; }
        public string LastMessage { get; set; }
        public string LastMessageAuthor { get; set; }
        public DateTime LastMessageTime { get; set; }
    }
}
