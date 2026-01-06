using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var products = await _productService.Find(filter);
            return Ok(products);
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var product = await _productService.Get(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.Create(request);
            if (result == null)
                return BadRequest();

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        // PUT: api/Product/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] ProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.Edit(id, request);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // PATCH: api/Product/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, [FromBody] ProductPatchRequest request)
        {
            var result = await _productService.Patch(id, request);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var success = await _productService.Remove(id);
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
