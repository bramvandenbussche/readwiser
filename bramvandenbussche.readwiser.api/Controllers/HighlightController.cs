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
        private readonly INoteRepository _repository;

        private readonly ILogger<HighlightController> _logger;

        public HighlightController(ILogger<HighlightController> logger, INoteRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetAll()
        {
            var data = await _repository.GetAll();

            return Ok(data.ToDto());
        }

        [HttpGet("book")]
        [Authorize]
        public async Task<ActionResult> GetHighlightsForBook(string title, string author)
        {
            var data = await _repository.GetForBook(title, author);

            return Ok(data.ToDto());
        }

        [HttpPost()]
        [Authorize]
        public ActionResult AddNewHighlight([FromBody] CreateHighlightRequestDto request)
        {
            var json = JsonSerializer.Serialize(request);
            _logger.LogDebug(json);
            foreach (var note in request.Highlights)
            {
                _repository.Save(note.ToDomain());
            }

            return Ok();
        }
    }
}