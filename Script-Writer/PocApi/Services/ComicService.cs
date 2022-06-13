using PocApi.Models;
using PocApi.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace PocApi.Services;

public class ComicService : IComicService
{
    private readonly IMongoCollection<Comic> _comicsCollection;

    public ComicService(
        IOptions<ComicConnectionSettings> bookStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            bookStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            bookStoreDatabaseSettings.Value.DatabaseName);

        _comicsCollection = mongoDatabase.GetCollection<Comic>("comic");
    }

    /// <summary>
    /// Gets all available comics.
    /// </summary>
    /// <returns>A list of comics.</returns>
    public async Task<List<Comic>> GetAll() =>
        await _comicsCollection.Find(_ => true).ToListAsync();

    /// <summary>
    /// Gets the comic for the provided id.
    /// </summary>
    /// <param name="id">The identifier of the comic.</param>
    /// <returns>A comic matching the id.</returns>
    public async Task<Comic?> GetWithId(string id) =>
        await _comicsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    /// <summary>
    /// Creates a new comic.
    /// </summary>
    /// <param name="newComic">The new comic.</param>
    /// <returns>The new comic.</returns>
    public async Task Create(Comic newComic) =>
        await _comicsCollection.InsertOneAsync(newComic);

    /// <summary>
    /// Updates the comic with the provided id.
    /// </summary>
    /// <param name="id">The identifier of the comic.</param>
    /// <param name="updatedComic">The comic data to update.</param>
    /// <returns>The updated comic.</returns>
    public async Task Update(string id, Comic updatedComic) =>
        await _comicsCollection.ReplaceOneAsync(x => x.Id == id, updatedComic);

    /// <summary>
    /// Deletes the comic with the provided id.
    /// </summary>
    /// <param name="id">The identifier of the comic.</param>
    /// <returns>If the deletion was succesfull.</returns>
    public async Task Delete(string id) =>
        await _comicsCollection.DeleteOneAsync(x => x.Id == id);
}