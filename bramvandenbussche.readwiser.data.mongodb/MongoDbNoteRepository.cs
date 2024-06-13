using bramvandenbussche.readwiser.data.mongodb.Model;
using bramvandenbussche.readwiser.domain.Interface.DataAccess;
using bramvandenbussche.readwiser.domain.Model;
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
        {
            
            var data = await _noteCollection.Find(_ => true).ToListAsync();

            return data.Select(note => note.AsDomain());
        }

        public async Task<IEnumerable<Highlight>> GetForBook(string title, string author)
        {
            var data = await _noteCollection.Find(x => x.Title == title && x.Author == author).ToListAsync();

            return data.Select(note => note.AsDomain());
        }

        public async Task Save(Highlight note)
        {
            var newNote = new StoredNote()
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                Title = note.Title,
                Author = note.Author,
                Chapter = note.Chapter,
                Text = note.Text,
                Note = note.Note
            };

            await _noteCollection.InsertOneAsync(newNote);
        }
    }
}