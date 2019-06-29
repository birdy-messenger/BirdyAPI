using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdyAPI.Dto;
using Newtonsoft.Json;

namespace BirdyAPI.Tools
{
    public static class ExceptionExtensions
    {
        public static ExceptionDto SerializeAsResponse(this Exception exception)
        {
            return new ExceptionDto(exception.Message);
        }
    }
}
