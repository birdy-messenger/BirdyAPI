using BirdyAPI.DataBaseModels;
using BirdyAPI.Models;
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
