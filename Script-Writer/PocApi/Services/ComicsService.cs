using PocApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace PocApi.Services;

public class ComicsService
{
    private readonly IMongoCollection<Comic> _comicsCollection;

    public ComicsService(
        IOptions<ComicStoreDatabaseSettings> bookStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            bookStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            bookStoreDatabaseSettings.Value.DatabaseName);

        _comicsCollection = mongoDatabase.GetCollection<Comic>(
            bookStoreDatabaseSettings.Value.ComicsCollectionName);
    }

    public async Task<List<Comic>> GetAsync() =>
        await _comicsCollection.Find(_ => true).ToListAsync();

    public async Task<Comic?> GetAsync(string id) =>
        await _comicsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Comic newComic) =>
        await _comicsCollection.InsertOneAsync(newComic);

    public async Task UpdateAsync(string id, Comic updatedComic) =>
        await _comicsCollection.ReplaceOneAsync(x => x.Id == id, updatedComic);

    public async Task RemoveAsync(string id) =>
        await _comicsCollection.DeleteOneAsync(x => x.Id == id);
}