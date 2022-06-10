using PocApi.Models;
using PocApi.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace PocApi.Services;

public class PageService : IPageService
{
    private readonly IMongoCollection<Page> pageCollection;

    public PageService(
        IOptions<ComicConnectionSettings> bookStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            bookStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            bookStoreDatabaseSettings.Value.DatabaseName);

        pageCollection = mongoDatabase.GetCollection<Page>("page");
    }

    /// <summary>
    /// Gets the page for the provided id.
    /// </summary>
    /// <param name="id">The identifier of the page.</param>
    /// <returns>A page matching the id.</returns>
    public async Task<Page?> GetWithId(string id) =>
        await pageCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    /// <summary>
    /// Gets all available issues for a specified issue.
    /// </summary>
    /// <returns>A list of pages.</returns>
    public async Task<List<Page>> GetAllForIssue(string issueId) =>
        await pageCollection.Find(x => x.IssueId == issueId).ToListAsync();    

    /// <summary>
    /// Creates a new page.
    /// </summary>
    /// <param name="newIssue">The new page.</param>
    /// <returns>If the creation was succesfull.</returns>
    public async Task Create(Page newPage) =>
        await pageCollection.InsertOneAsync(newPage);

    /// <summary>
    /// Updates the page with the provided id.
    /// </summary>
    /// <param name="id">The identifier of the page.</param>
    /// <param name="updatedIssue">The page data to update.</param>
    /// <returns>If the update was succesfull.</returns>
    public async Task Update(string id, Page updatedPage) =>
        await pageCollection.ReplaceOneAsync(x => x.Id == id, updatedPage);

    /// <summary>
    /// Deletes the page with the provided id.
    /// </summary>
    /// <param name="id">The identifier of the page.</param>
    /// <returns>If the deletion was succesfull.</returns>
    public async Task Delete(string id) =>
        await pageCollection.DeleteOneAsync(x => x.Id == id);
}