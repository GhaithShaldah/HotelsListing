using HotelsListing.Configurations.EntitiesAuthorization;
using HotelsListing.Configurations.EntitiesConfiguration;
using HotelsListing.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelsListing.Data
{
    public class DataBaseContext : IdentityDbContext<ApiUser>
    {
        public DataBaseContext(DbContextOptions options) : base(options)
        {
                
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new CountryConfiguration());
            builder.ApplyConfiguration(new HotelConfiguration());
            builder.ApplyConfiguration(new RolesConfiguration());  
        } 

        public DbSet<Country> Countries  { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

    }
}
