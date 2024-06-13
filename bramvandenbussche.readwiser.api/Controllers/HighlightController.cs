using System.Text.Json;
using bramvandenbussche.readwiser.api.Contract;
using bramvandenbussche.readwiser.domain.Interface.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace bramvandenbussche.readwiser.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HighlightController : ControllerBase
    {
        private readonly IHighlightService _service;
        //private readonly INoteMigrationService _migrationService;

        private readonly ILogger<HighlightController> _logger;
        private readonly MemoryCache _cache = new(new MemoryCacheOptions());

        public HighlightController(IHighlightService service, ILogger<HighlightController> logger) //, INoteMigrationService migrationService)
        {
            _service = service;
            _logger = logger;
            //_migrationService = migrationService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetAll(int timestamp = 0)
        {
            _logger.LogDebug($"{nameof(GetAll)}: Request received with timestamp: {timestamp}");
            try
            {
                var data = await _service.GetAll(timestamp);

                _logger.LogInformation($"{nameof(GetAll)}: Returned {data.Count()} notes");
                return Ok(data.ToApiContract());
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return Problem(e.Message, statusCode: 500);
            }
        }

        [HttpGet("book")]
        [Authorize]
        public async Task<ActionResult> GetHighlightsForBook(string title, string author)
        {
            _logger.LogDebug($"{nameof(GetHighlightsForBook)}: Request received for book: {author} - {title}");
            var data = await _service.GetForBook(title, author);

            _logger.LogInformation($"{nameof(GetHighlightsForBook)}: Returned {data.Count()} notes");
            return Ok(data.ToApiContract());
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddNewHighlight([FromBody] CreateHighlightRequest request)
        {
            _logger.LogDebug($"{nameof(AddNewHighlight)}: Request received containing {request.Highlights.Count} notes. First book: {request.Highlights[0].Author} - {request.Highlights[0].Title}");
            
            if (!request.IsValid)
                return BadRequest("Invalid request object");

            try
            {
                await _service.Add(request.Highlights.ToDomain());
                _logger.LogInformation($"{nameof(AddNewHighlight)}: Saved {request.Highlights.Count} notes");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            

            return Ok();
        }

        //[HttpPost("migrate")]
        //[Authorize]
        //public async Task<ActionResult> MigrateStorage([FromBody] MigrateNotesRequest source)
        //{
        //    _logger.LogDebug($"{nameof(MigrateStorage)}: Request received with timestamp: {source.Timestamp}");

        //    if (!source.IsValid)
        //        return BadRequest("Invalid request object");

        //    var result = await _migrationService.ImportAll(source.SourceConnection, source.Timestamp);

        //    if (result.IsSucces)
        //    {
        //        _logger.LogInformation(
        //            $"{nameof(MigrateStorage)}: Migration finished. Imported {result.Notes} notes for {result.Books} books.");
        //        return Ok(result);
        //    }

        //    _logger.LogError($"{nameof(MigrateStorage)}: Migration failed. Reason: {result.Reason}.");
        //    return Problem(result.Reason);
        //}
    }
}