using PocApi.Models;
using PocApi.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace PocApi.Services;

public class ActorService : IActorService
{
    private readonly IMongoCollection<Actor> actorCollection;

    public ActorService(
        IOptions<ComicConnectionSettings> bookStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            bookStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            bookStoreDatabaseSettings.Value.DatabaseName);

        actorCollection = mongoDatabase.GetCollection<Actor>("actor");
    }

    /// <summary>
    /// Gets the actor for the provided id.
    /// </summary>
    /// <param name="id">The identifier of the actor.</param>
    /// <returns>A panel matching the id.</returns>
    public async Task<Actor?> GetWithId(string id) =>
        await actorCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    /// <summary>
    /// Gets all available actors for the specified comic id.
    /// </summary>
    /// <param name="comicId">The identifier of the comic.</param>
    /// <returns>A list of actors.</returns>
    public async Task<List<Actor>> GetAllForComic(string comicId) =>
        await actorCollection.Find(x => x.ComicId == comicId).ToListAsync();

    /// <summary>
    /// Creates a new actor.
    /// </summary>
    /// <param name="newIssue">The new actor.</param>
    /// <returns>If the creation was succesfull.</returns>
    public async Task Create(Actor newActor) =>
        await actorCollection.InsertOneAsync(newActor);

    /// <summary>
    /// Updates the actor with the provided id.
    /// </summary>
    /// <param name="id">The identifier of the actor.</param>
    /// <param name="updatedIssue">The actor data to update.</param>
    /// <returns>If the update was succesfull.</returns>
    public async Task Update(string id, Actor updatedActor) =>
        await actorCollection.ReplaceOneAsync(x => x.Id == id, updatedActor);

    /// <summary>
    /// Deletes the actor with the provided id.
    /// </summary>
    /// <param name="id">The identifier of the actor.</param>
    /// <returns>If the deletion was succesfull.</returns>
    public async Task Delete(string id) =>
        await actorCollection.DeleteOneAsync(x => x.Id == id);
}