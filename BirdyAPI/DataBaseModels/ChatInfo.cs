using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BirdyAPI.DataBaseModels
{
    public class ChatInfo
    {
        [Key]
        public Guid ChatID { get; set; }
        public string ChatName { get; set; }
    }
}
