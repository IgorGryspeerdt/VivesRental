using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VivesRental.Services.Abstractions;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;

namespace VivesRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] CustomerFilter? filter = null)
        {
            var customers = await _customerService.Find(filter);
            return Ok(customers);
        }

        // GET: api/Customer/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var customer = await _customerService.Get(id);
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }

        // POST: api/Customer
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _customerService.Create(request);
            if (result == null)
                return BadRequest();

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        // PUT: api/Customer/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] CustomerRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _customerService.Edit(id, request);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // PATCH: api/Customer/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, [FromBody] CustomerPatchRequest request)
        {
            var result = await _customerService.Patch(id, request);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // DELETE: api/Customer/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var success = await _customerService.Remove(id);
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