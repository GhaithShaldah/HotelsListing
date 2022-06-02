using AutoMapper;
using HotelsListing.Authentication_Services;
using HotelsListing.Data.Entities;
using HotelsListing.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HotelsListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IAuthenticationManager _authManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;

        public AccountController(
                                UserManager<ApiUser> userManager
                                ,IAuthenticationManager authManager
                                , ILogger<AccountController> logger
                                , IMapper mapper
                                 )
        {
            _userManager = userManager;
            _authManager = authManager;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDto) {
            _logger.LogInformation($"Registeration attempt for {userDto.Email}");
            if (!ModelState.IsValid) { 
            
               return BadRequest(ModelState);
            }
            try
            {
                var user = _mapper.Map<ApiUser>(userDto);
                user.UserName = userDto.Email;
                var result = await _userManager.CreateAsync(user,userDto.Password);
                if (!result.Succeeded) 
                {
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError(error.Code, error.Description);
                      }
                    return BadRequest(ModelState);
                }
                await _userManager.AddToRolesAsync(user, userDto.Roles);

                return Accepted();
            }

            catch (Exception ex) 
            {
                _logger.LogError(ex,$"Something went wrong in the {nameof(Register)}");
                return Problem($"Something went wrong in the {nameof(Register)}",statusCode : 500);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LogIn([FromBody] LogInDTO logInDTO)
        {

            _logger.LogInformation($"Login attempt for {logInDTO.Email}");
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }
            try
            {
                if (!await _authManager.ValidateUser(logInDTO)) {
                    return Unauthorized();
                }

                return Accepted(new { token = await _authManager.CreateToken()});
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(LogIn)}");
                return Problem($"Something went wrong in the {nameof(LogIn)}", statusCode: 500);
            }
        }
    }
}
