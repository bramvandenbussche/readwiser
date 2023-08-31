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
        public async Task<ActionResult> GetAll(int timestamp = 0)
        {
            var data = await _repository.GetAll(timestamp);

            return Ok(data.ToDto());
        }

        [HttpGet("book")]
        [Authorize]
        public async Task<ActionResult> GetHighlightsForBook(string title, string author)
        {
            var data = await _repository.GetForBook(title, author);

            return Ok(data.ToDto());
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddNewHighlight([FromBody] CreateHighlightRequestDto request)
        {
            var cache = new Dictionary<string, List<Highlight>>();
            var makeKey = (string author, string title) => $"{author}-{title}";
            
            foreach (var note in request.Highlights)
            {
                var key = makeKey(note.Author, note.Title);
                if (!cache.ContainsKey(key))
                {
                    var data = await _repository.GetForBook(note.Title, note.Author);
                    cache.Add(key, data.ToList());
                }

                // Prevent duplicates
                if (!cache[key].Any(x => x.Chapter == note.Chapter && x.Text == note.Text))
                {
                    var value = note.ToDomain();
                    cache[key].Add(value);
                    await _repository.Save(value);
                }
            }

            return Ok();
        }
    }
}