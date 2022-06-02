using HotelsListing.Data.Entities;
using HotelsListing.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelsListing.Authentication_Services
{
    public class AuthenticationManager : IAuthenticationManager
    {

        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private ApiUser _user; 

        public AuthenticationManager(UserManager<ApiUser> userManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        
        public async Task<string> CreateToken()
        {
            var signingCreadentials = GenerateSigningCredantials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokensOptions(signingCreadentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials GenerateSigningCredantials() {
            var Key = Environment.GetEnvironmentVariable("KEY");
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        
        }

        private async Task<List<Claim>> GetClaims()
        {
            var Claims = new List<Claim> {
               new Claim(ClaimTypes.Name,_user.UserName)
            };
            var roles = await _userManager.GetRolesAsync(_user);

            foreach (var role in roles) {
                Claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return Claims;

        }

        private JwtSecurityToken GenerateTokensOptions(SigningCredentials signingCreadentials, List<Claim> claims) {
            var JwtSettings = _configuration.GetSection("Jwt");
            var expiration = DateTime.Now.AddMinutes(
                             Convert.ToDouble(JwtSettings.GetSection("LifeTime").Value));
            var options = new JwtSecurityToken(
                issuer: JwtSettings.GetSection("Issuer").Value,
                claims:claims,
                expires: expiration,
                signingCredentials:signingCreadentials
                  );

            return options;
           
        }
        public async Task<bool> ValidateUser(LogInDTO userDto)
        {
            _user = await _userManager.FindByNameAsync(userDto.Email);
            return (_user !=null && await _userManager.CheckPasswordAsync(_user,userDto.Password));
        }
    }
}
