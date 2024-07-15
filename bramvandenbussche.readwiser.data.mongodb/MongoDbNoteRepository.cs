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

            collection.Indexes.CreateOne(new CreateIndexModel<StoredNote>(
                Builders<StoredNote>
                    .IndexKeys.Ascending(n => n.Tags)
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
        {
            var data =  (await _noteCollection
                    .Find(_ => true)
                    .SortByDescending(n => n.Timestamp)
                    .Limit(amount)
                    .ToListAsync());

                return data
                .Select(note => note.AsDomain());
        }


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
                Note = note.Note,
                Tags = new List<string>()
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



        public async Task<List<string>?> GetAllTags()
        {
            FieldDefinition<StoredNote, string> field = "Tags";
            var filter = new FilterDefinitionBuilder<StoredNote>().Exists(field, true);
            
            return await _noteCollection.Distinct(field, filter).ToListAsync();
        }

        public async Task AddTag(string noteId, string tag)
        {
            var bsonNoteId = new BsonObjectId(new ObjectId(noteId));

            var update = Builders<StoredNote>.Update.AddToSet(note => note.Tags, tag);
            await _noteCollection.UpdateOneAsync(note => note.Id == bsonNoteId, update);
        }

        public async Task RemoveTag(string noteId, string tag)
        {
            var bsonNoteId = new BsonObjectId(new ObjectId(noteId));

            var update = Builders<StoredNote>.Update.Pull(note => note.Tags, tag);
            await _noteCollection.UpdateOneAsync(note => note.Id == bsonNoteId, update);
        }
    }
}