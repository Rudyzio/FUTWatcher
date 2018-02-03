using FUTWatcher.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FUTWatcher.API.Data {
    public class ApplicationDbContext : DbContext
    {
        protected ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ApplicationDbContextModelBuilder.BuildModel(builder);
        }

        public DbSet<Club> Clubs { get; set; }

        public DbSet<League> Leagues { get; set; }

        public DbSet<Nation> Nations { get; set; }

        public DbSet<PlayerType> PlayerTypes { get; set; }

        public DbSet<PlayerVersion> PlayerVersions { get; set; }
        public DbSet<DailyProfit> DailyProfits { get; set; }
        public DbSet<User> Users { get; set; }
    }
}