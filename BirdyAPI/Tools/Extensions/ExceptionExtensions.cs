using System;
using BirdyAPI.Dto;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace BirdyAPI.Tools.Extensions
{
    public static class ExceptionExtensions
    {
        public static string Serialize(this ExceptionDto ex)
        {
            return JsonConvert.SerializeObject(ex);
        }
    }
}
