using HotelsListing.DTOs.CountryDTOs;

namespace HotelsListing.DTOs.HotelDTOs
{
    public class HotelDTO :HotelCreatedDTO
    {
        public int Id { get; set; }
        public CountryDTO Country { get; set; }
    }
}
