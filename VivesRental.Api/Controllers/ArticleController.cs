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
            try
            {
                var articles = await _articleService.Find(filter);
                return Ok(articles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/Article/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var article = await _articleService.Get(id);
                if (article == null)
                    return NotFound(new { error = "Article not found." });
                return Ok(article);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST: api/Article
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ArticleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _articleService.Create(request);
                if (result == null)
                    return BadRequest(new { error = "Could not create article." });

                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PATCH: api/Article/{id}/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] ArticleStatus status)
        {
            try
            {
                var success = await _articleService.UpdateStatus(id, status);
                if (!success)
                    return NotFound(new { error = "Article not found or status update failed." });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // DELETE: api/Article/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var success = await _articleService.Remove(id);
                if (!success)
                    return NotFound(new { error = "Article not found or could not be deleted." });
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(new { error = "Article already deleted or concurrency error." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}