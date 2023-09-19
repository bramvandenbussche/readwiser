using System.Text.Json;
using bramvandenbussche.readwiser.api.DataAccess;
using bramvandenbussche.readwiser.api.Domain;
using bramvandenbussche.readwiser.api.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bramvandenbussche.readwiser.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HighlightController : ControllerBase
    {
        private readonly IHighlightService _service;

        private readonly ILogger<HighlightController> _logger;

        public HighlightController(ILogger<HighlightController> logger, IHighlightService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetAll(int timestamp = 0)
        {
            var data = await _service.GetAll(timestamp);

            return Ok(data.ToDto());
        }

        [HttpGet("book")]
        [Authorize]
        public async Task<ActionResult> GetHighlightsForBook(string title, string author)
        {
            var data = await _service.GetForBook(title, author);

            return Ok(data.ToDto());
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddNewHighlight([FromBody] CreateHighlightRequestDto request)
        {
            try
            {
                await _service.Add(request.Highlights);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            

            return Ok();
        }

        [HttpPost("migrate")]
        [Authorize]
        public async Task<ActionResult> MigrateStorage([FromBody] string source)
        {

            return Ok();
        }
    }
}