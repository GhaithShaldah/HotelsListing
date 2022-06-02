using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelsListing.DTOs
{
    public class UserDTO : LogInDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string  PhoneNumber { get; set; }
        public ICollection<string> Roles{ get; set; }

    }
}
