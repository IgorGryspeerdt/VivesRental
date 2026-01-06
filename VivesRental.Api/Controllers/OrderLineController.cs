using Microsoft.AspNetCore.Mvc;
using VivesRental.Services.Abstractions;
using VivesRental.Services.Model.Filters;

namespace VivesRental.Api.Controllers
{
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
            var orderLines = await _orderLineService.Find(filter);
            return Ok(orderLines);
        }

        // GET: api/OrderLine/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var orderLine = await _orderLineService.Get(id);
            if (orderLine == null)
                return NotFound();
            return Ok(orderLine);
        }
    }
}