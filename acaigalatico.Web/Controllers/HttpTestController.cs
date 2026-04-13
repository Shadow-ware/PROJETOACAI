using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using acaigalatico.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace acaigalatico.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HttpTestController : ControllerBase
    {
        private readonly IApiService _apiService;
        private readonly ILogger<HttpTestController> _logger;

        public HttpTestController(IApiService apiService, ILogger<HttpTestController> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        [HttpGet("posts")]
        public async Task<IActionResult> GetExternalPosts()
        {
            try
            {
                var posts = await _apiService.GetPostsAsync();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no endpoint GetExternalPosts");
                return StatusCode(500, new { message = "Erro interno ao buscar posts", details = ex.Message });
            }
        }

        [HttpPost("create-post")]
        public async Task<IActionResult> CreateExternalPost([FromBody] dynamic postData)
        {
            try
            {
                var result = await _apiService.CreatePostAsync(postData);
                return CreatedAtAction(nameof(GetExternalPosts), result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no endpoint CreateExternalPost");
                return StatusCode(500, new { message = "Erro interno ao criar post", details = ex.Message });
            }
        }
    }
}
