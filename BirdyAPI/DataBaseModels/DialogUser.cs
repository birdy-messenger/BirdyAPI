using System;
using System.ComponentModel.DataAnnotations;

namespace BirdyAPI.DataBaseModels
{
    public class DialogUser
    {
        [Key]
        public Guid DialogID { get; set; }
        public int FirstUserID { get; set; }
        public int SecondUserID { get; set; }
    }
}
