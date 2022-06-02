using HotelsListing.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelsListing.Configurations.EntitiesConfiguration
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
                 new Country()
                 {
                     Id = 1,
                     Name = "Jamaica",
                     ShortName = "JK"
                 },
                  new Country()
                  {
                      Id = 2,
                      Name = "Bahamas",
                      ShortName = "BH"
                  },
                  new Country()
                  {
                      Id = 3,
                      Name = "Cayman Island",
                      ShortName = "CI"
                  }
                );
        }
    }
}
