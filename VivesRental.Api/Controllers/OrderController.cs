using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using VivesRental.Services.Abstractions;
using VivesRental.Services.Model.Filters;

namespace VivesRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] OrderFilter? filter = null)
        {
            try
            {
                var orders = await _orderService.Find(filter);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/Order/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var order = await _orderService.Get(id);
                if (order == null)
                    return NotFound(new { error = "Order not found." });
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST: api/Order?customerId={customerId}
        [HttpPost]
        [Authorize(Roles = "Medewerker")]
        public async Task<IActionResult> Create([FromQuery] Guid customerId)
        {
            try
            {
                var result = await _orderService.Create(customerId);
                if (result == null)
                    return BadRequest(new { error = "Could not create order. Customer may not exist." });

                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new { error = aex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PATCH: api/Order/{id}/return
        // Mark all order lines for this order as returned (sets ReturnedAt).
        [HttpPatch("{id}/return")]
        [Authorize(Roles = "Medewerker")]
        public async Task<IActionResult> Return(Guid id, [FromBody] DateTime? returnedAt)
        {
            try
            {
                var when = returnedAt ?? DateTime.Now;
                var success = await _orderService.Return(id, when);
                if (!success)
                    return NotFound(new { error = "Order not found or nothing to return." });

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { error = "Concurrency error while returning the order." });
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new { error = aex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}