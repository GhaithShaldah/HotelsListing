using AutoMapper;
using HotelsListing.Data.Entities;
using HotelsListing.DTOs;
using HotelsListing.DTOs.CountryDTOs;
using HotelsListing.DTOs.HotelDTOs;

namespace HotelsListing.Configurations
{
    public class MapperInitlizer :Profile
    {
        public MapperInitlizer()
        {
              CreateMap<Country,CountryDTO>().ReverseMap();
              CreateMap<Country,CountryCreateDTO>().ReverseMap();
              CreateMap<Hotel, HotelDTO>().ReverseMap();
              CreateMap<Hotel, HotelCreatedDTO>().ReverseMap();
              CreateMap<ApiUser, UserDTO>().ReverseMap();
        }
    }
}
