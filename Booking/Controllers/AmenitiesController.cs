using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Controllers
{
    [ApiController]
    [Route("api/amenities/[Action]")]
    public class AmenitiesController : ControllerBase
    {
        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok();
        }
    }
}
