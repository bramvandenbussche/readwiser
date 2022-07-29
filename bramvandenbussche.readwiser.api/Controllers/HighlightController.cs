using System.Collections.Concurrent;
using bramvandenbussche.readwiser.api.Dto;
using Microsoft.AspNetCore.Mvc;

namespace bramvandenbussche.readwiser.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HighlightController : ControllerBase
    {
        private static ConcurrentBag<HighlightDto> _cache = new();

        private readonly ILogger<HighlightController> _logger;

        public HighlightController(ILogger<HighlightController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_cache.ToList());
        }
        
        [HttpPost()]
        public ActionResult AddNewHighlight([FromBody] HighlightRequestDto request)
        {
            foreach (var h in request.Highlights)
            {
                _cache.Add(h);
            }

            return Ok(_cache.Count);
        }
    }
}