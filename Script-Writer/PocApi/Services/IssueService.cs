using PocApi.Models;
using PocApi.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace PocApi.Services;

public class IssueService : IIssueService
{
    private readonly IMongoCollection<Issue> issueCollection;

    public IssueService(
        IOptions<ComicStoreDatabaseSettings> bookStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            bookStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            bookStoreDatabaseSettings.Value.DatabaseName);

        issueCollection = mongoDatabase.GetCollection<Issue>("issue");
    }

    /// <summary>
    /// Gets all available issues for a specified comic.
    /// </summary>
    /// <returns>A list of issues.</returns>
    public async Task<List<Issue>> GetAllForComic(string id) =>
        await issueCollection.Find(x => x.Id == id).ToListAsync();

    /// <summary>
    /// Gets the issue for the provided id.
    /// </summary>
    /// <param name="id">The identifier of the issue.</param>
    /// <returns>A issue matching the id.</returns>
    public async Task<Issue?> GetWithId(string id) =>
        await issueCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    /// <summary>
    /// Creates a new issue.
    /// </summary>
    /// <param name="newIssue">The new issue.</param>
    /// <returns>The new issue.</returns>
    public async Task Create(Issue newIssue) =>
        await issueCollection.InsertOneAsync(newIssue);

    /// <summary>
    /// Updates the issue with the provided id.
    /// </summary>
    /// <param name="id">The identifier of the issue.</param>
    /// <param name="updatedIssue">The issue data to update.</param>
    /// <returns>If the update was succesfull.</returns>
    public async Task Update(string id, Issue updatedIssue) =>
        await issueCollection.ReplaceOneAsync(x => x.Id == id, updatedIssue);

    /// <summary>
    /// Deletes the issue with the provided id.
    /// </summary>
    /// <param name="id">The identifier of the issue.</param>
    /// <returns>If the deletion was succesfull.</returns>
    public async Task Delete(string id) =>
        await issueCollection.DeleteOneAsync(x => x.Id == id);
}