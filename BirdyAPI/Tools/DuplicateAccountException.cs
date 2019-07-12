using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BirdyAPI.Tools
{
    public class DuplicateAccountException : Exception
    {
        public DuplicateAccountException(string message) : base(message)
        { }
    }
}
