using System;

namespace BirdyAPI.Tools.Exceptions
{
    public class DataNotFoundException : Exception
    {
        public DataNotFoundException(string message) : base(message) { }
    }
}
