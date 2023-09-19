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
        private readonly INoteMigrationService _migrationService;

        private readonly ILogger<HighlightController> _logger;

        public HighlightController(IHighlightService service, INoteMigrationService migrationService, ILogger<HighlightController> logger)
        {
            _service = service;
            _logger = logger;
            _migrationService = migrationService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetAll(int timestamp = 0)
        {
            _logger.LogDebug($"{nameof(GetAll)}: Request received with timestamp: {timestamp}");
            var data = await _service.GetAll(timestamp);

            _logger.LogInformation($"{nameof(GetAll)}: Returned {data.Count()} notes");
            return Ok(data.ToDto());
        }

        [HttpGet("book")]
        [Authorize]
        public async Task<ActionResult> GetHighlightsForBook(string title, string author)
        {
            _logger.LogDebug($"{nameof(GetHighlightsForBook)}: Request received for book: {author} - {title}");
            var data = await _service.GetForBook(title, author);

            _logger.LogInformation($"{nameof(GetHighlightsForBook)}: Returned {data.Count()} notes");
            return Ok(data.ToDto());
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddNewHighlight([FromBody] CreateHighlightRequestDto request)
        {
            _logger.LogDebug($"{nameof(AddNewHighlight)}: Request received containing {request.Highlights.Count} notes. First book: {request.Highlights[0].Author} - {request.Highlights[0].Title}");
            try
            {
                await _service.Add(request.Highlights);
                _logger.LogInformation($"{nameof(AddNewHighlight)}: Saved {request.Highlights.Count} notes");
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
        public async Task<ActionResult> MigrateStorage([FromBody] MigrateNotesRequest source)
        {
            _logger.LogDebug($"{nameof(MigrateStorage)}: Request received with timestamp: {source.Timestamp}");
            var result = await _migrationService.ImportAll(source.SourceConnection, source.Timestamp);

            if (result.IsSucces)
            {
                _logger.LogInformation(
                    $"{nameof(MigrateStorage)}: Migration finished. Imported {result.Notes} notes for {result.Books} books.");
                return Ok(result);
            }

            _logger.LogError($"{nameof(MigrateStorage)}: Migration failed. Reason: {result.Reason}.");
            return Problem(result.Reason);
        }
    }
}