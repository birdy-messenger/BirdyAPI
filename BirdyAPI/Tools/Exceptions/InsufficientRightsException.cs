using System;

namespace BirdyAPI.Tools.Exceptions
{
    public class InsufficientRightsException : Exception
    {
        public InsufficientRightsException(string message) : base(message) { }
    }
}
