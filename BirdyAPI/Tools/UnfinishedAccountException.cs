using System;

namespace BirdyAPI.Tools
{
    public class UnfinishedAccountException : Exception
    {
        public UnfinishedAccountException(string message) : base(message) { }
    }
}
