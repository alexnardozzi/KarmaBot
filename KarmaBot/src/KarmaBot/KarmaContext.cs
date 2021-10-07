using KarmaBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KarmaBot
{
    public class KarmaContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        
        private readonly IConfiguration _configuration;

        public KarmaContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;
            
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("WebApiDatabase"));
            base.OnConfiguring(optionsBuilder);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Karma)
                .WithOne(k => k.User)
                .HasForeignKey<Karma>(k => k.UserId);
        }
    }
}