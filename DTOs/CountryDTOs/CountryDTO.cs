using HotelsListing.DTOs.HotelDTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelsListing.DTOs.CountryDTOs
{
    public class CountryDTO : CountryCreateDTO
    {
        public int Id { get; set; }
        
    }
}
