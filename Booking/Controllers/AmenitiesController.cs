using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Controllers
{
    [ApiController]
    [Route("api/amenities/[Action]")]
    public class AmenitiesController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok();
        }
    }
}
