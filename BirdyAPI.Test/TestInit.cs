using BirdyAPI.Controllers;
using BirdyAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace BirdyAPI.Test
{
    public class TestInit
    {
        public static AppEntryService _AppEntryService;
        static TestInit()
        {
            var builder =
                new DbContextOptionsBuilder<BirdyContext>().UseSqlServer(string.Empty);

            BirdyContext context = new BirdyContext(builder.Options);
            _AppEntryService = new AppEntryService(context);
        }
    }
}