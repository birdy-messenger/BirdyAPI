using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BirdyAPI.DataBaseModels
{
    public class Friends
    {
        public Friends() { }
        [Key]
        public int FirstUserID { get; set; }
        public int SecondUserID { get; set; }
    }
}
