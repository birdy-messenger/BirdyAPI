using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BirdyAPI.Dto
{
    public class ExceptionDto
    {
        public ExceptionDto(string message)
        {
            ErrorMessage = message;
        }
        public string ErrorMessage { get; set; }
    }
}
