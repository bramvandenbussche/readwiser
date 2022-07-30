using System.Collections.Concurrent;
using bramvandenbussche.readwiser.api.DataAccess;
using bramvandenbussche.readwiser.api.Domain;
using bramvandenbussche.readwiser.api.Dto;
using Microsoft.AspNetCore.Mvc;

namespace bramvandenbussche.readwiser.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HighlightController : ControllerBase
    {
        private static ConcurrentBag<HighlightDto> _cache = new();
        private readonly INoteRepository _repository;

        private readonly ILogger<HighlightController> _logger;

        public HighlightController(ILogger<HighlightController> logger, INoteRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var data = await _repository.GetAll();
            return Ok(data.ToList());
        }
        
        [HttpPost()]
        public ActionResult AddNewHighlight([FromBody] HighlightRequestDto request)
        {
            foreach (var note in request.Highlights)
            {
                _cache.Add(note);
                _repository.Save(note.ToDomain());
            }

            return Ok(_cache.Count);
        }
    }
}