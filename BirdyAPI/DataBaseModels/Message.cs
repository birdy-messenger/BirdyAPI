using System;

namespace BirdyAPI.DataBaseModels
{
    public class Message
    {
        public Guid ChatID { get; set; }
        public Guid MessageID { get; set; }
        public int AuthorID { get; set; }
        public DateTime SendDate { get; set; }
        public string Text { get; set; }
    }
}
