using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BirdyAPI.Dto
{
    public class SimpleAnswerDto
    {
        public SimpleAnswerDto(string message)
        {
            Result = message;
        }
        public string Result { get; set; }
    }
}
