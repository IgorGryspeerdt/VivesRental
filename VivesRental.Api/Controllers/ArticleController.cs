using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VivesRental.Enums;
using VivesRental.Services.Abstractions;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;

namespace VivesRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        // GET: api/Article
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ArticleFilter? filter = null)
        {
            var articles = await _articleService.Find(filter);
            return Ok(articles);
        }

        // GET: api/Article/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var article = await _articleService.Get(id);
            if (article == null)
                return NotFound();
            return Ok(article);
        }

        // POST: api/Article
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ArticleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _articleService.Create(request);
            if (result == null)
                return BadRequest();

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        // PATCH: api/Article/{id}/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] ArticleStatus status)
        {
            var success = await _articleService.UpdateStatus(id, status);
            if (!success)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/Article/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var success = await _articleService.Remove(id);
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