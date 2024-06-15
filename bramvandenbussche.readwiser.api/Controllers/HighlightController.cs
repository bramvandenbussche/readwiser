using System.ComponentModel.DataAnnotations;
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

        private readonly ILogger<HighlightController> _logger;
        private readonly MemoryCache _cache = new(new MemoryCacheOptions());

        public HighlightController(IHighlightService service, ILogger<HighlightController> logger)
        {
            _service = service;
            _logger = logger;
        }

        #region Calibre Annotations

        /// <summary>
        /// Get all highlights since the given timestamp. If timestamp is not provided, all notes will be returned.
        /// </summary>
        /// <param name="timestamp">Optional starting point to filter data by</param>
        /// <returns></returns>
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

        /// <summary>
        /// Retrieve all notes for a given book (author + title)
        /// </summary>
        /// <param name="title">Title of the book</param>
        /// <param name="author">Author of the book</param>
        /// <returns></returns>
        [HttpGet("book")]
        [Authorize]
        public async Task<ActionResult> GetHighlightsForBook(string title, string author)
        {
            _logger.LogDebug($"{nameof(GetHighlightsForBook)}: Request received for book: {author} - {title}");
            var data = await _service.GetForBook(title, author);

            _logger.LogInformation($"{nameof(GetHighlightsForBook)}: Returned {data.Count()} notes");
            return Ok(data.ToApiContract());
        }

        #endregion

        #region MoonReader Pro - ReadWise API

        /// <summary>
        /// Create a new highlight.
        /// Called from MoonReader.
        /// Implements the ReadWise API.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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

        #endregion


        /// <summary>
        /// Get a list of authors with notes
        /// </summary>
        [HttpGet("authors")]
        public async Task<ActionResult> GetAuthors()
        {
            _logger.LogDebug($"{nameof(GetAuthors)}: Request received for authors");
            var data = await _service.GetAllAuthors();

            _logger.LogInformation($"{nameof(GetAuthors)}: Returned {data.Count()} authors");
            return Ok(data);
        }
        

        /// <summary>
        /// Get a list of books and notes for a given author
        /// </summary>
        /// <param name="name">The name of the author</param>
        [HttpGet("author")]
        public async Task<ActionResult> GetBooksForAuthor([FromQuery, Required] string name)
        {
            _logger.LogDebug($"{nameof(GetBooksForAuthor)}: Request received for authors");
            var data = await _service.GetBooksForAuthor(name);

            _logger.LogInformation($"{nameof(GetBooksForAuthor)}: Returned {data.Count()} authors");
            return Ok(data);
        }
    }
}