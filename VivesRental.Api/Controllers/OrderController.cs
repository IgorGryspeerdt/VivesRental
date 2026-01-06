using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var orders = await _orderService.Find(filter);
            return Ok(orders);
        }

        // GET: api/Order/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var order = await _orderService.Get(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        // POST: api/Order?customerId={customerId}
        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] Guid customerId)
        {
            var result = await _orderService.Create(customerId);
            if (result == null)
                return BadRequest();

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }
    }
}