using HotelsListing.DTOs.HotelDTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelsListing.DTOs.CountryDTOs
{
    public class CountryCreateDTO
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Error!. Country name is too long")]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 2, ErrorMessage = "Error!.Country Short Name is too long")]
        public string ShortName { get; set; }
        
        [Required]
        public IList<HotelDTO> Hotels { get; set; }



    }
}
