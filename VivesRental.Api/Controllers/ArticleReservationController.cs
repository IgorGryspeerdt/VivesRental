using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VivesRental.Services.Abstractions;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;

namespace VivesRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleReservationController : ControllerBase
    {
        private readonly IArticleReservationService _reservationService;

        public ArticleReservationController(IArticleReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        // GET: api/ArticleReservation
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ArticleReservationFilter? filter = null)
        {
            var reservations = await _reservationService.Find(filter);
            return Ok(reservations);
        }

        // GET: api/ArticleReservation/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var reservation = await _reservationService.Get(id);
            if (reservation == null)
                return NotFound();
            return Ok(reservation);
        }

        // POST: api/ArticleReservation
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ArticleReservationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _reservationService.Create(request);
            if (result == null)
                return BadRequest();

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        // DELETE: api/ArticleReservation/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var success = await _reservationService.Remove(id);
                if (!success)
                    return NotFound();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }
    }
}