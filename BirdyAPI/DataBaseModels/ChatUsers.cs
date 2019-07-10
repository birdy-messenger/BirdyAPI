using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BirdyAPI.DataBaseModels
{
    public class ChatUsers
    {
        public Guid ChatID { get; set; }
        public int UserInChatID { get; set; }
    }
}
