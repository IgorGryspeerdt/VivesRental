using Microsoft.AspNetCore.Http;
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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ProductFilter? filter = null)
        {
            try
            {
                var products = await _productService.Find(filter);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var product = await _productService.Get(id);
                if (product == null)
                    return NotFound(new { error = "Product not found." });
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST: api/Product
        [HttpPost]
        [Authorize(Roles = "Medewerker")]
        public async Task<IActionResult> Create([FromBody] ProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _productService.Create(request);
                if (result == null)
                    return BadRequest(new { error = "Could not create product." });

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

        // PUT: api/Product/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Medewerker")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] ProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _productService.Edit(id, request);
                if (result == null)
                    return NotFound(new { error = "Product not found." });

                return Ok(result);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new { error = aex.Message });
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(new { error = "Product already deleted or concurrency error." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PATCH: api/Product/{id}
        [HttpPatch("{id}")]
        [Authorize(Roles = "Medewerker")]
        public async Task<IActionResult> Patch(Guid id, [FromBody] ProductPatchRequest request)
        {
            try
            {
                var result = await _productService.Patch(id, request);
                if (result == null)
                    return NotFound(new { error = "Product not found." });

                return Ok(result);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new { error = aex.Message });
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(new { error = "Product already deleted or concurrency error." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Medewerker")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var success = await _productService.Remove(id);
                if (!success)
                    return NotFound(new { error = "Product not found or could not be deleted." });
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(new { error = "Product already deleted or concurrency error." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
