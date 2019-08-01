using Microsoft.EntityFrameworkCore;

namespace BirdyAPI.Test
{
    public class TestContext
    {
        public static BirdyContext GetContext()
        {
            var options = new DbContextOptionsBuilder<BirdyContext>()
                .UseSqlite(
                    "Data Source=BirdyTestDB.db")
                .Options;

            return new BirdyContext(options);
        }
    }
}