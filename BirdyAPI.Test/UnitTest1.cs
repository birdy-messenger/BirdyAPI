using System;
using BirdyAPI.Controllers;
using BirdyAPI.Dto;
using BirdyAPI.Services;
using Xunit;

namespace BirdyAPI.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }

        private static readonly AppEntryController test;

        static UnitTest1()
        {
            test = TestInit._AppEntryService;
        }

        [Fact]
        public void TestMethod1()
        {
            test.UserRegistration(new RegistrationDto
                {Email = "vladkazanskiy@mail.ru", FirstName = "Vlad", PasswordHash = "fhwouigvsvj"});
        }
    }
}
