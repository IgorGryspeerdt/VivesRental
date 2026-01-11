using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
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
            try
            {
                var customers = await _customerService.Find(filter);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/Customer/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var customer = await _customerService.Get(id);
                if (customer == null)
                    return NotFound(new { error = "Customer not found." });
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST: api/Customer
        [HttpPost]
        [Authorize(Roles = "Medewerker")]
        public async Task<IActionResult> Create([FromBody] CustomerRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _customerService.Create(request);
                if (result == null)
                    return BadRequest(new { error = "Could not create customer." });

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

        // PUT: api/Customer/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Medewerker")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] CustomerRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _customerService.Edit(id, request);
                if (result == null)
                    return NotFound(new { error = "Customer not found." });

                return Ok(result);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new { error = aex.Message });
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(new { error = "Customer already deleted or concurrency error." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PATCH: api/Customer/{id}
        [HttpPatch("{id}")]
        [Authorize(Roles = "Medewerker")]
        public async Task<IActionResult> Patch(Guid id, [FromBody] CustomerPatchRequest request)
        {
            try
            {
                var result = await _customerService.Patch(id, request);
                if (result == null)
                    return NotFound(new { error = "Customer not found." });

                return Ok(result);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new { error = aex.Message });
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(new { error = "Customer already deleted or concurrency error." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // DELETE: api/Customer/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Medewerker")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var success = await _customerService.Remove(id);
                if (!success)
                    return NotFound(new { error = "Customer not found or could not be deleted." });
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(new { error = "Customer already deleted or concurrency error." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}