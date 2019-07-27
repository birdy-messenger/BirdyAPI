using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BirdyAPI.Dto
{
    public class MessageDto
    {
        public string AuthorUniqueTag { get; set; }
        public string Message { get; set; }
        public DateTime MessageTime { get; set; }
    }
}
