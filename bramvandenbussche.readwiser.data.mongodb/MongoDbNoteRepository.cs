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
            EnsureDatabaseExists(database);

            _noteCollection = database.GetCollection<StoredNote>(COLLECTION_NAME);

            EnsureIndexExists(_noteCollection);
        }

        private static void EnsureIndexExists(IMongoCollection<StoredNote> collection)
        {
            // Create index (if it does not exist - if it does, server will simply return a success command)
            collection.Indexes.CreateOne(new CreateIndexModel<StoredNote>(
                Builders<StoredNote>
                    .IndexKeys
                    .Ascending(n => n.Author)
                    .Ascending(n => n.Title)
            ));
        }

        private static void EnsureDatabaseExists(IMongoDatabase database)
        {
            var databaseName = database.DatabaseNamespace.DatabaseName;
            var dbs = database.Client.ListDatabaseNames().ToList();
            if (!dbs.Contains(databaseName))
            {
                var db = database.Client.GetDatabase(databaseName);
            }
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

            var update = Builders<StoredNote>.Update.Set(note => note.Note, highlight.Note);
            await _noteCollection.UpdateOneAsync(note => note.Id == noteId, update);
        }

        public async Task DeleteHighlight(string noteId)
        {
            var id = new BsonObjectId(new ObjectId(noteId));

            await _noteCollection.DeleteOneAsync(note => note.Id == id);
        }
    }
}