using BirdyAPI.DataBaseModels;
using Microsoft.EntityFrameworkCore;

namespace BirdyAPI
{
    public class BirdyContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<UserSessions> UserSessions { get; set; }
        public DbSet<ChatUsers> ChatUsers { get; set; }
        public DbSet<ChatInfo> ChatInfo { get; set; }
        public DbSet<Message> Messages { get; set; }
        public BirdyContext(DbContextOptions<BirdyContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friend>().HasKey(k => new {k.FirstUserID, k.SecondUserID});
            modelBuilder.Entity<ChatUsers>().HasKey(k => new {k.ChatID, k.UserInChatID});
        }
    }
}
