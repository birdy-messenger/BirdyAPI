using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BirdyAPI.Dto
{
    public class FriendRequestAnswerDto
    {
        public FriendRequestAnswerDto(string answer)
        {
            FriendRequestResult = answer;
        }
        public string FriendRequestResult { get; set; }
    }
}
