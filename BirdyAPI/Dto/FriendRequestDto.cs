using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BirdyAPI.Dto
{
    public class FriendRequestDto
    {
        public int OutgoingUserID { get; set; }
        public int IncomingUserID { get; set; }
    }
}
