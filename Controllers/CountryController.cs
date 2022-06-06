using AutoMapper;
using HotelsListing.Data.Entities;
using HotelsListing.DTOs;
using HotelsListing.DTOs.CountryDTOs;
using HotelsListing.DTOs.HotelDTOs;
using HotelsListing.Repository_Pattern.IRepository;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelsListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;

        public CountryController(IUnitOfWork unitofwork, ILogger<CountryController> logger,IMapper mapper)
        {
            _unitOfWork = unitofwork;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public,MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        
        public async Task<IActionResult> GetCountries([FromQuery] RequestParams requestParams) {
         
           try
            {
                var countries = await _unitOfWork.Countries.GetPagedList(requestParams);
                var CountriesDTO = _mapper.Map<IList<CountryDTO>>(countries);
                return Ok(CountriesDTO);
            }
            catch(Exception ex)
            {
               _logger.LogError(ex,$"Something went wrong in the {nameof(GetCountries)}");
                return StatusCode(500,"Intenal Server Error..Please try again");
            }
        }

        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetCountry(int id) {
            try
            {
                var Country = await _unitOfWork.Countries
                      .Get(q => q.Id == id,new List<string> {"Hotels"});

                var result = _mapper.Map<CountryDTO>(Country);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCountry)}");
                return StatusCode(500, "Intenal Server Error..Please try again");

            }
          
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]

        public async Task<IActionResult> CreateCountry(CountryDTO countryDto) {

            if (!ModelState.IsValid) {
               return BadRequest(ModelState);
            }

            try
            {
                var country = _mapper.Map<Country>(countryDto);
                await _unitOfWork.Countries.Insert(country);
                await _unitOfWork.Save();
                return CreatedAtAction("GetCountry", new { id = country.Id }, country);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(CreateCountry)}");
                return StatusCode(500, "Internal Server Error..Please try again");
            }
        }

        [HttpPut("{Id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] CountryUpdatedDTO countryDto)
        {
            if (!ModelState.IsValid || id < 1) {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateCountry)}");
                return BadRequest(ModelState);

            }

            try
            {
                var country = await _unitOfWork.Countries.Get(c => c.Id == id);
                if (country == null) { 
                   return BadRequest("Submitted data is invalid");
                }
                _mapper.Map(countryDto, country);
                _unitOfWork.Countries.Update(country);
                await _unitOfWork.Save();
                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateCountry)}");   
                return StatusCode(500, "Internal Server error...Please try again");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid Delete attempt in {nameof(DeleteCountry)}");
                return BadRequest();
            }
            try
            {
                var hotel = await _unitOfWork.Countries.Get(h => h.Id == id);
                if (hotel == null)
                {
                    _logger.LogError($"Invalid Delete attempt in {nameof(DeleteCountry)}");
                    return BadRequest();
                }
                await _unitOfWork.Countries.Delete(id);
                await _unitOfWork.Save();
                return NoContent();

            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteCountry)}");
                return StatusCode(500, "Internal Server error...Please try again");
            }
        }
    }
}