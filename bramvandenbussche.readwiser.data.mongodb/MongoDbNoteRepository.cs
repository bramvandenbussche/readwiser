using bramvandenbussche.readwiser.data.mongodb.Model;
using bramvandenbussche.readwiser.domain.Interface.DataAccess;
using bramvandenbussche.readwiser.domain.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace bramvandenbussche.readwiser.data.mongodb
{
    public class MongoDbNoteRepository : INoteRepository
    {
        private const string COLLECTION_NAME = "notes";

        private readonly IMongoCollection<StoredNote> _noteCollection;

        public MongoDbNoteRepository(IMongoDatabase database)
        {
            _noteCollection = database.GetCollection<StoredNote>(COLLECTION_NAME);
        }

        public async Task<IEnumerable<Highlight>> GetAll(int timestamp)
            => (await _noteCollection.Find(_ => true)
                    .ToListAsync())
                .Select(note => note.AsDomain());


        public async Task<IEnumerable<Highlight>> GetForBook(string title, string author)
            => (await _noteCollection.Find(x => x.Title == title && x.Author == author)
                    .ToListAsync())
                .Select(note => note.AsDomain());


        public async Task<IEnumerable<Highlight>> GetNotesForAuthor(string author) 
            => (await _noteCollection.Find(x => x.Author == author)
                    .ToListAsync())
                .Select(note => note.AsDomain());


        public async Task<IEnumerable<string>> GetAllAuthors()
            => await _noteCollection.Distinct(x => x.Author, _ => true).ToListAsync();


        public async Task<IEnumerable<Highlight>> GetRecent(int amount)
            => (await _noteCollection
                .Find(_ => true)
                .SortByDescending(n => n.Timestamp)
                .Limit(amount)
                .ToListAsync())
                .Select(note => note.AsDomain());


        public async Task Save(Highlight note)
        {
            var newNote = new StoredNote()
            {
                //Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                Title = note.Title,
                Author = note.Author,
                Chapter = note.Chapter,
                Text = note.Text,
                Note = note.Note
            };

            await _noteCollection.InsertOneAsync(newNote);
        }

        public async Task UpdateHighlight(Highlight highlight)
        {
            var noteId = new BsonObjectId(new ObjectId(highlight.NoteId));
            var existing = _noteCollection.Find(note => note.Id == noteId).FirstOrDefault();
            var update = Builders<StoredNote>.Update.Set(note => note.Note, highlight.Note);
            await _noteCollection.UpdateOneAsync(note => note.Id == noteId, update);
        }
    }
}