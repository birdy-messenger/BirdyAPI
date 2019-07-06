using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BirdyAPI.DataBaseModels
{
    public class Friends
    {
        public Friends() { }
        public int FirstUserID { get; set; }
        public int SecondUserID { get; set; }
        public bool RequestAccepted { get; set; } 
    }
}
