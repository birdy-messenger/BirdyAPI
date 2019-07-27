using System;

namespace BirdyAPI.Tools.Exceptions
{
    public class DuplicateAccountException : Exception
    {
        public DuplicateAccountException(string message) : base(message) { }
    }
}
