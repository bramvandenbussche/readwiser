using System.ComponentModel.DataAnnotations;

namespace bramvandenbussche.readwiser.api.Dto;

public class CreateHighlightRequestDto
{
    public List<HighlightDto> Highlights { get; set; } = new();

    public class HighlightDto
    {
        /// <summary>
        /// The highlight text, (technically the only field required in a highlight object)
        /// Required
        /// </summary>
        [Required]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Title of the book/article/podcast (the "source")
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Author of the book/article/podcast (the "source")
        /// </summary>
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// The url of a cover image for this book/article/podcast (the "source")
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// The url of the article/podcast (the "source")
        /// </summary>
        public string? SourceUrl { get; set; }

        /// <summary>
        /// A meaningful unique identifier for your app (string between 3 and 64 chars, no spaces).
        /// Example: my_app.
        /// (Note: for legacy integrations book/article/podcast can also be passed here, but it is not recommended anymore.)
        /// </summary>
        public string? SourceType { get; set; }

        /// <summary>
        /// One of: 'books', 'articles', 'tweets' or 'podcasts'. This will determine where the highlight shows in the user's dashboard and some aspects of how we render it.
        /// If category is not provided we will assume that the category is either 'articles' (if source_url is provided) or(otherwise) a 'books'.
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// Annotation note attached to the specific highlight.
        /// You can also use this field to create tags thanks to our inline tagging functionality.
        /// </summary>
        public string? Note { get; set; }

        /// <summary>
        /// Highlight's location in the source text. Used to order the highlights.
        /// If not provided we will fill this based on the order of highlights in the list.
        /// If location_type is 'podcast', we interpret the integer as number of seconds that elapsed from the start of the recording.
        /// </summary>
        public int Location { get; set; }

        /// <summary>
        /// One of: 'page', 'order' or 'time_offset'
        /// Default is 'order'.If provided type is different than 'order', make sure to provide location as well (see below).
        /// </summary>
        public string? LocationType { get; set; }

        /// <summary>
        /// A datetime representing when the highlight was taken in the ISO 8601 format; default timezone is UTC.
        /// Example: "2020-07-14T20:11:24+00:00"
        /// </summary>
        public string? HighlightedAt { get; set; }

        /// <summary>
        /// Unique url of the specific highlight (eg. a concrete tweet or a podcast snippet)
        /// </summary>
        public string? HighlightUrl { get; set; }

        public string? Chapter { get; set; }
    }
}