using HotelsListing.Data.Entities;
using HotelsListing.DTOs.HotelDTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelsListing.DTOs.CountryDTOs
{
    public class CountryUpdatedDTO : CountryCreateDTO
    {
        public IList<HotelCreatedDTO> Hotels { get; set; }
    }
}
