using HotelsListing.Data;
using HotelsListing.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace HotelsListing.Configurations
{
    public static class ServicesExtension
    {
        public static void ConfigureIdentity(this IServiceCollection service)
        {
            var builder = service.AddIdentityCore<ApiUser>(q => q.User.RequireUniqueEmail = true);
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole),service);
            builder.AddEntityFrameworkStores<DataBaseContext>().AddDefaultTokenProviders();
        }

        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration) {
            var JwtSettings = configuration.GetSection("Jwt");
            var Key = Environment.GetEnvironmentVariable("KEY");

            services.AddAuthentication(
                   o =>
                   {
                       o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                       o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                   }
                ).AddJwtBearer(
                   o =>
                   {
                       o.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuer = true,
                           ValidateLifetime = true,
                           ValidateIssuerSigningKey = true,
                           ValidIssuer = JwtSettings.GetSection("Issuer").Value,
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key))
                       };
                   }
                );
                 
                
                }
    }
}
