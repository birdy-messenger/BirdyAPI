using System;
using BirdyAPI.Controllers;
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

        private readonly AppEntryService test;

        public UnitTest1()
        {
            test = TestInit._AppEntryService;
        }

        [Fact]
        public void TestMethod1()
        {

        }
    }
}
