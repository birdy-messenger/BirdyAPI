using BirdyAPI.DataBaseModels;
using Microsoft.EntityFrameworkCore;

namespace BirdyAPI
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        { }
    }
}
