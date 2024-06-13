using System.Runtime.Serialization;
using bramvandenbussche.readwiser.domain.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace bramvandenbussche.readwiser.data.mongodb.Model;

public class StoredNote
{
    [BsonId]
    [DataMember]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId _id { get; set; }

    [DataMember]
    public Guid Id { get; set; }

    [DataMember]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Book title
    /// </summary>
    [DataMember]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Book author
    /// </summary>
    [DataMember]
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// The highlighted text
    /// </summary>
    [DataMember]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Optional note
    /// </summary>
    [DataMember]
    public string? Note { get; set; }

    /// <summary>
    /// The chapter where the highlight was made
    /// </summary>
    [DataMember]
    public string Chapter { get; set; } = string.Empty;

    public Highlight AsDomain() =>
        new()
        {
            Author = Author,
            Title = Title,
            Chapter = Chapter,
            Note = Note,
            Text = Text,
            NoteId = Id,
            RaisedTime = Timestamp
        };
}