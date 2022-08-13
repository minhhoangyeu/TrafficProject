using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Traffic.Data.EF;
using Traffic.Data.EF.Configurations;
using Traffic.Data.Entities;
using Traffic.Utilities.Helpers;
using System;
using System.IO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Traffic.Data.EF
{
    public class TrafficContext : IdentityDbContext<User, Role, Guid>
    {
        public TrafficContext(DbContextOptions<TrafficContext> options) : base(options)
        {

        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    //builder.AddConfiguration(new CardToDeliveryConfiguration());
            
        //}
        public DbSet<CardToDelivery> CardToDelivery { set; get; }

        
    }

    //public class TrafficContextFactory : IDesignTimeDbContextFactory<TrafficContext>
    //{
    //    public TrafficContext CreateDbContext(string[] args)
    //    {
    //        IConfiguration configuration = new ConfigurationBuilder()
    //            .SetBasePath(Directory.GetCurrentDirectory())
    //            .AddJsonFile("appsettings.json").Build();
    //        var connectionString = Cryptography.DecryptString(configuration.GetConnectionString("DefaultConnection"));            
    //        var optionsBuilder = new DbContextOptionsBuilder<TrafficContext>();
    //        optionsBuilder.Use .UseOracle(connectionString, o => o.MigrationsAssembly("Traffic.Data.EF")).UseUpperSnakeCaseNamingConvention();

    //        return new TrafficContext(optionsBuilder.Options);
    //    }
    //}
}