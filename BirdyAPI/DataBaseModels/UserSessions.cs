using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BirdyAPI.DataBaseModels
{
    public class UserSessions
    {
        [Key]
        public Guid Token { get; set; }
        public int UserId { get; set; }
    }
}
