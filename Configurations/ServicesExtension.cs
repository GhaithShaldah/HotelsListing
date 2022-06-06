using AspNetCoreRateLimit;
using HotelsListing.Data;
using HotelsListing.Data.Entities;
using HotelsListing.DTOs;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
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

        public static void ConfigureExceptionHandler(this IApplicationBuilder app) {

            app.UseExceptionHandler(error =>
                 error.Run(async context =>
                 {
                     context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                     context.Response.ContentType = "application/json";
                     var contextFeature =
                     context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();

                     if (contextFeature != null)
                     {
                         Log.Error($"Something went wrong in the {contextFeature.Error}");

                         await context.Response.WriteAsync(new ErrorHandler
                         {
                             StatusCode = context.Response.StatusCode,
                             Message = ("Internal Server Error..Please try again")
                         }.ToString());

                     }
                 })
                 );
             
        }

        public static void ConfigureVersioning(this IServiceCollection services) {

            services.AddApiVersioning(options =>
               {
                   options.ReportApiVersions = true;
                   options.AssumeDefaultVersionWhenUnspecified = true;
                   options.DefaultApiVersion = new ApiVersion(1,0);
                   options.ApiVersionReader = new HeaderApiVersionReader("api-version");
               }

            );
        }
        public static void ConfigureHttpCacheHeader(this IServiceCollection services) {

            services.AddResponseCaching();
            services.AddHttpCacheHeaders(
                (expirationOpt) => {
                    expirationOpt.MaxAge = 120;
                    expirationOpt.CacheLocation = CacheLocation.Private;

                },
                (validationOpt) => { 
                  validationOpt.MustRevalidate = true;
                 }
                );
        
        }

        public static void ConfigureRateLimiting(this IServiceCollection services) {

            var rateLimitRules = new List<RateLimitRule>
                  {
                      new RateLimitRule{
                         Endpoint = "*",
                         Limit  = 100,
                         Period = "10s"
                      }
                  };

            services.Configure<IpRateLimitOptions>(opt =>
                 {

                     opt.GeneralRules = rateLimitRules; 
                 });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration,RateLimitConfiguration>();
                
        
        }
    }
}
