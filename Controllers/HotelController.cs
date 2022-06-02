using AutoMapper;
using HotelsListing.Data.Entities;
using HotelsListing.DTOs.HotelDTOs;
using HotelsListing.Repository_Pattern.IRepository;
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
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly ILogger<HotelController> _logger;
        private readonly IMapper _mapper;

        public HotelController(IUnitOfWork unitOfWork, ILogger<HotelController> logger, IMapper mapper)
        {
            _unitofwork = unitOfWork;
            _logger = logger;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> GetHotels()
        {
            try
            {
                var Hotels = await _unitofwork.Hotels.GetAll();
                var HotelsDto = _mapper.Map<IList<HotelDTO>>(Hotels);
                return Ok(HotelsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetHotels)}");
                return StatusCode(500, "Intenal Server Error..Please try again");
            }
        }

        [HttpGet("{id}", Name = "GetHotel")]


        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                var Hotel = await _unitofwork.Hotels.Get(q => q.Id == id, new List<string> { "Country" });
                var HotelDto = _mapper.Map<HotelDTO>(Hotel);
                return Ok(HotelDto);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetHotel)}");
                return StatusCode(500, "Intenal Server Error..Please try again");
            }

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]

        public async Task<IActionResult> CreateHotel(HotelCreatedDTO hotelDto)
        {

            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid Post attempt in{nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }
            try
            {
                var hotel = _mapper.Map<Hotel>(hotelDto);
                await _unitofwork.Hotels.Insert(hotel);
                await _unitofwork.Save();
                return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(CreateHotel)}");
                return StatusCode(500, "Internal Server Error..Please try again");
            }
        }

        [HttpPut("{id:int}")]


        public async Task<IActionResult> UpdateHotel(int id, [FromBody] HotelUpdatedDTO hotelDto)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateHotel)}");
                return BadRequest(ModelState);
            }
            try
            {
                var hotel = await _unitofwork.Hotels.Get(q => q.Id == id);
                if (hotel == null)
                {
                    return BadRequest("Submitted data is invalid");
                }

                _mapper.Map(hotelDto, hotel);
                _unitofwork.Hotels.Update(hotel);
                await _unitofwork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateHotel)}");
                return StatusCode(500, "Internal Server Error.Please Try Again");
            }

        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid Delete attempt in {nameof(DeleteHotel)}");
                return BadRequest();
            }
            try
            {
                var hotel = await _unitofwork.Hotels.Get(h => h.Id == id);
                if (hotel == null)
                {
                    _logger.LogError($"Invalid Delete attempt in {nameof(DeleteHotel)}");
                    return BadRequest();
                }
                await _unitofwork.Hotels.Delete(id);
                await _unitofwork.Save();
                return NoContent();

            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteHotel)}");
                return StatusCode(500, "Internal Server error...Please try again");
            }
        }

    }
    }


