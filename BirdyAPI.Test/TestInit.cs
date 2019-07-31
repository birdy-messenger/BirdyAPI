using Microsoft.EntityFrameworkCore;

namespace BirdyAPI.Test
{
    public class TestInit
    {
        public static readonly DbContextOptions<BirdyContext> Options;
        static TestInit()
        {
            Options = new DbContextOptionsBuilder<BirdyContext>()
                .UseSqlite(
                    "Data Source=BirdyTestDB.db")
                .Options;
        }
    }
}