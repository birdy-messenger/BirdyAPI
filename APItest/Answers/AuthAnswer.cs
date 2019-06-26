using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.Answers;

namespace BirdyAPI.Answers
{
    public class LoginAnswer : Answer
    {
        public int Id { get; set; }
        public int Token { get; set; }
        public string ErrorMessage { get; set; }
    }
}
