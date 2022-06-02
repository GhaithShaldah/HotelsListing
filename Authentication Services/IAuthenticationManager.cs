using HotelsListing.DTOs;
using System.Threading.Tasks;

namespace HotelsListing.Authentication_Services
{
    public interface IAuthenticationManager
    {
        Task<bool> ValidateUser(LogInDTO userDto);
        Task<string> CreateToken();
    }
}
