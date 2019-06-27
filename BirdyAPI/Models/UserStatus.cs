using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BirdyAPI.Models
{
    public class UserStatus
    {
        public enum Status
        {
            Unconfirmed = 0,
            Confirmed = 1
        }
    }
}
