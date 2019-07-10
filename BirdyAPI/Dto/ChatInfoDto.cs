using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BirdyAPI.Dto
{
    public class ChatInfoDto
    {
        public Guid ChatID { get; set; }
        public string ChatName { get; set; }
        public string LastMessage { get; set; }
    }
}
