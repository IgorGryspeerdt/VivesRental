using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using VivesRental.Services.Abstractions;
using VivesRental.Services.Model.Filters;

namespace VivesRental.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderLineController : ControllerBase
{
    private readonly IOrderLineService _orderLineService;

    public OrderLineController(IOrderLineService orderLineService)
    {
        _orderLineService = orderLineService;
    }

    // GET: api/OrderLine
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] OrderLineFilter? filter = null)
    {
        try
        {
            var orderLines = await _orderLineService.Find(filter);
            return Ok(orderLines);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    // GET: api/OrderLine/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            var orderLine = await _orderLineService.Get(id);
            if (orderLine == null)
                return NotFound(new { error = "OrderLine not found." });
            return Ok(orderLine);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    // PATCH: api/OrderLine/{id}/return
    // Mark a single orderline as returned (sets ReturnedAt).
    [HttpPatch("{id}/return")]
    [Authorize(Roles = "Medewerker")]
    public async Task<IActionResult> Return(Guid id, [FromBody] DateTime? returnedAt)
    {
        try
        {
            var when = returnedAt ?? DateTime.Now;
            var success = await _orderLineService.Return(id, when);
            if (!success)
                return NotFound(new { error = "OrderLine not found or already returned." });

            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(new { error = "Concurrency error while returning the order line." });
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