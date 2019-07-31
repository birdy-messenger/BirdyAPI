using BirdyAPI.Services;
using Xunit;

namespace BirdyAPI.Test
{
    public class DebugServiceTest
    {
        private static readonly DebugService DebugService;
        static DebugServiceTest()
        {
            BirdyContext context = new BirdyContext(TestInit.Options);
            DebugService = new DebugService(context);
        }
        [Fact]
        public void EmptyTest_Ok()
        {

        }

        [Fact]
        public void GetUsers_Ok()
        {
            DebugService.GetAllUsers();
        }
    }
}
