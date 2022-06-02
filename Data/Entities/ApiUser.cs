using Microsoft.AspNetCore.Identity;

namespace HotelsListing.Data.Entities
{
    public class ApiUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
