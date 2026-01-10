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
            try
            {
                var reservations = await _reservationService.Find(filter);
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/ArticleReservation/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var reservation = await _reservationService.Get(id);
                if (reservation == null)
                    return NotFound(new { error = "Reservation not found." });
                return Ok(reservation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST: api/ArticleReservation
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ArticleReservationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _reservationService.Create(request);
                if (result == null)
                    return BadRequest(new { error = "Could not create reservation." });

                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (ArgumentException aex)
            {
                // For example: validation from service layer
                return BadRequest(new { error = aex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // DELETE: api/ArticleReservation/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var success = await _reservationService.Remove(id);
                if (!success)
                    return NotFound(new { error = "Reservation not found or could not be deleted." });
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(new { error = "Reservation already deleted or concurrency error." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}