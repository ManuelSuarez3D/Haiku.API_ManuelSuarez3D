using Haiku.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Haiku.API.Repositories
{
    public class HaikuAPIContext : DbContext
    {
        public HaikuAPIContext() { }
        public HaikuAPIContext(DbContextOptions<HaikuAPIContext> options) : base(options) { }
        public DbSet<HaikuItem> HaikuItems { get; set; }
        public DbSet<CreatorItem> Creators { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HaikuItem>()
                .HasOne<CreatorItem>()
                .WithMany()
                .HasForeignKey(h => h.CreatorId);

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("1234");
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "dev1", Password = hashedPassword }
            );

            modelBuilder.Entity<CreatorItem>().HasData(
                new CreatorItem { Id = 1, Name = "Unknown" },
                new CreatorItem { Id = 2, Name = "Bashō", Bio = "Born Matsuo Kinsaku, later known as Matsuo Chūemon Munefusa was the most famous Japanese poet of the Edo period." }
            );
            modelBuilder.Entity<HaikuItem>().HasData(
                new HaikuItem { Id = 1, Title = "An Old Silent Pond", LineOne = "An old silent pond...", LineTwo = "A frog jumps into the pond,", LineThree = "splash! Silence again.", CreatorId = 2 }
            );
        }
    }
}
