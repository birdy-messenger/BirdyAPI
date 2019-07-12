using System;
using BirdyAPI.Dto;

namespace BirdyAPI.Tools
{
    public static class ExceptionExtensions
    {
        public static ExceptionDto SerializeAsResponse(this Exception exception)
        {
            return new ExceptionDto{ErrorMessage = exception.Message};
        }
    }
}
