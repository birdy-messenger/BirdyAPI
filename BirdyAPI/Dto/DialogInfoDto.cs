using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BirdyAPI.Dto
{
    public class DialogInfoDto
    {
        public string InterlocutorUniqueTag { get; set; }
        public string LastMessage { get; set; }
        public string LastMessageAuthor { get; set; }
        public DateTime LastMessageTime { get; set; }
    }
}
