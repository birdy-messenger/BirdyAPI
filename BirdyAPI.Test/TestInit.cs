using BirdyAPI.Controllers;
using BirdyAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace BirdyAPI.Test
{
    public class TestInit
    {
        public static AppEntryController _AppEntryService;
        static TestInit()
        {
            var builder =
                new DbContextOptionsBuilder<BirdyContext>().UseSqlServer("Server=tcp:birdytest.database.windows.net,1433;Initial Catalog=BirdyDB;Persist Security Info=False;User ID=lol67;Password=Lolilop67;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            BirdyContext context = new BirdyContext(builder.Options);
            _AppEntryService = new AppEntryController(context);
        }
    }
}