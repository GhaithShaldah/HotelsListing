using HotelsListing.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelsListing.Configurations.EntitiesConfiguration
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
                   new Hotel()
                   {
                       Id = 1,
                       Name = "Sandals Report and Spa",
                       Address = "Negril",
                       CountryId = 1,
                       Rating = 4.5
                   },
                    new Hotel()
                    {
                        Id = 2,
                        Name = "Comfort Suits",
                        Address = "George Town",
                        CountryId = 3,
                        Rating = 4.3
                    },
                     new Hotel()
                     {
                         Id = 3,
                         Name = "Grand Pallidum",
                         Address = "Negril",
                         CountryId = 2,
                         Rating = 4
                     }
                );
        }
    }
}
