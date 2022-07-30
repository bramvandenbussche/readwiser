using System.Text.Json;
using bramvandenbussche.readwiser.api.DataAccess;
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
            return Ok(data.Select(x => new HighlightResponseDto()
            {
                Id = x.NoteId,
                Author = x.Author,
                Title = x.Title,
                HighlightText = x.Text,
                Location = x.Chapter,
                Timestamp = x.RaisedTime.ToString("s")
            }).ToList());
        }

        [HttpGet("book")]
        [Authorize]
        public async Task<ActionResult> GetHighlightsForBook(string title, string author)
        {
            var data = await _repository.GetForBook(title, author);
            return Ok(data.Select(x => new HighlightResponseDto()
            {
                Id = x.NoteId,
                Author = x.Author,
                Title = x.Title,
                HighlightText = x.Text,
                Location = x.Chapter,
                Timestamp = x.RaisedTime.ToString("s")
            }).ToList());
        }

        [HttpPost()]
        [Authorize]
        public ActionResult AddNewHighlight([FromBody] HighlightRequestDto request)
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