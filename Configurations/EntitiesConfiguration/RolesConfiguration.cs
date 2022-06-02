

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelsListing.Configurations.EntitiesAuthorization
{
    public class RolesConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                { 
                  Name = "User",
                  NormalizedName = "User"
                },
                new IdentityRole 
                {
                   Name = "Administrator",
                   NormalizedName = "ADMINISTRATOR"
                }
                );
        }
    }
}
