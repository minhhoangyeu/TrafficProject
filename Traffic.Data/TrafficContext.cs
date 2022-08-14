
using Traffic.Data.Entities;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Traffic.Data.Interfaces;

namespace Traffic.Data
{
    public class TrafficContext : DbContext
    {
        public TrafficContext(DbContextOptions<TrafficContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(b => b.UserName)
                 .IsUnique()
                .HasFilter(null);
            modelBuilder.Entity<User>()
               .HasIndex(b => b.IpAddress)
                .IsUnique()
               .HasFilter(null);
            modelBuilder.Entity<CampaignHistory>()
               .HasIndex(b => b.CampaignId)
                .IsUnique()
               .HasFilter(null);
            modelBuilder.Entity<UserCampaignConfig>()
               .HasIndex(b => b.LevelId)
                .IsUnique()
               .HasFilter(null);
            modelBuilder.Entity<UserCampaign>()
              .HasIndex(b => b.Token)
               .IsUnique()
              .HasFilter(null);
     
            modelBuilder.Entity<CampaignHistory>()
                        .HasOne(m => m.User)
                        .WithMany()
                        .HasForeignKey(m => m.ImplementBy)
                        .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserCampaign>()
                        .HasOne(m => m.User)
                        .WithMany()
                        .HasForeignKey(m => m.ImplementBy)
                        .OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<User> Users { set; get; }
        public DbSet<Campaign> Campaigns { set; get; }
        public DbSet<UserCampaignConfig> UserCampaignConfigs { set; get; }
        public DbSet<CampaignHistory> CampaignHistorys { set; get; }
        public DbSet<UserCampaign> UserCampaign { set; get; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<EntityEntry> modified = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
            foreach (EntityEntry item in modified)
            {
                if (item.Entity is ITracking changedOrAddedItem)
                {
                    if (item.State == EntityState.Added)
                    {
                        changedOrAddedItem.CreatedDate = DateTime.Now;
                    }
                    else
                    {
                        changedOrAddedItem.UpdatedDate = DateTime.Now;
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TrafficContext>
    {
        public TrafficContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<TrafficContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new TrafficContext(builder.Options);
        }
    }
}