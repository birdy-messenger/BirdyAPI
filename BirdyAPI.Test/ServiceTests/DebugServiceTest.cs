using BirdyAPI.Services;
using BirdyAPI.Test.Factories;
using Xunit;

namespace BirdyAPI.Test.ServiceTests
{
    public class DebugServiceTest
    {

        [Fact]
        public void EmptyTest_Ok()
        {

        }

        [Fact]
        public void GetUsers_Ok()
        {
            DebugService debugService = new DebugService(ContextFactory.GetContext());
            debugService.GetAllUsers();
        }
    }
}
