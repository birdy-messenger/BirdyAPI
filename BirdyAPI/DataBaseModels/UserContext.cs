using Microsoft.EntityFrameworkCore;

namespace BirdyAPI.Models
{
    //TODO:3 Context != Model
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        { }
    }
}
