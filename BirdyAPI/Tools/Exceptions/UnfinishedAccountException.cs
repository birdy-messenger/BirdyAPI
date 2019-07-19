using System;

namespace BirdyAPI.Tools.Exceptions
{
    public class UnfinishedAccountException : Exception
    {
        public UnfinishedAccountException(string message) : base(message) { }
    }
}
