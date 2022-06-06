using HotelsListing.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelsListing.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/Country")]
    [ApiController]
    public class CountryV2Controller : ControllerBase
    {
        private readonly DataBaseContext _context;
        public CountryV2Controller(DataBaseContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetCountries() {
            return Ok(_context.Countries);
        }
    }
}
