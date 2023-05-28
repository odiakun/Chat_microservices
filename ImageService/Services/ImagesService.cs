using ImageService.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ImageService.Services
{
    public class ImagesService
    {
        private readonly IMongoCollection<Image> _imagesCollection;

        public ImagesService(IOptions<ImagesDatabaseSettings> imagesDatabaseSettings)
        {
            var mongoClient = new MongoClient(imagesDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(imagesDatabaseSettings.Value.DatabaseName);

            _imagesCollection = mongoDatabase.GetCollection<Image>(imagesDatabaseSettings.Value.ImagesCollectionName);
        }

        public async Task<List<Image>> GetAsync() =>
            await _imagesCollection.Find(image => true).ToListAsync();

        // get single message async
        public async Task<Image> GetAsync(string id) =>
            await _imagesCollection.Find(image => image.mid == id).FirstOrDefaultAsync();

        // create message async
        public async Task CreateAsync(Image image) =>
            await _imagesCollection.InsertOneAsync(image);

        // update message async
        public async Task UpdateAsync(string id, Image updatedImage) =>
            await _imagesCollection.ReplaceOneAsync(image => image.mid == id, updatedImage);

        // delete message async
        public async Task DeleteAsync(string id) =>
            await _imagesCollection.DeleteOneAsync(image => image.mid == id);
    }
}
