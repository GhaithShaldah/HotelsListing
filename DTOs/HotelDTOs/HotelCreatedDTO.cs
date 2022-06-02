using HotelsListing.Data.Entities;
using System.ComponentModel.DataAnnotations;


namespace HotelsListing.DTOs.HotelDTOs
{
    public class HotelCreatedDTO
    {
        [Required]
        [StringLength(maximumLength:150,ErrorMessage = "Error!..Hotel Name is too long")]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength:250,ErrorMessage = "Error..Hotel Address is too long")]
        public string Address { get; set; }
        [Range(1,5)]
        public double Rating { get; set; }
        [Required]
        public int CountryId { get; set; }
    }
}
