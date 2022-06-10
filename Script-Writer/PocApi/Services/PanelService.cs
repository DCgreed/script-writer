using PocApi.Models;
using PocApi.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace PocApi.Services;

public class PanelService : IPanelService
{
    private readonly IMongoCollection<Panel> panelCollection;

    public PanelService(
        IOptions<ComicConnectionSettings> bookStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            bookStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            bookStoreDatabaseSettings.Value.DatabaseName);

        panelCollection = mongoDatabase.GetCollection<Panel>("panel");
    }

    /// <summary>
    /// Gets the panel for the provided id.
    /// </summary>
    /// <param name="id">The identifier of the panel.</param>
    /// <returns>A panel matching the id.</returns>
    public async Task<Panel?> GetWithId(string id) =>
        await panelCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    /// <summary>
    /// Gets all available panels for a specified page.
    /// </summary>
    /// <returns>A list of panel.</returns>
    public async Task<List<Panel>> GetAllForPage(string pageId) =>
        await panelCollection.Find(x => x.PageId == pageId).ToListAsync();    

    /// <summary>
    /// Creates a new panel.
    /// </summary>
    /// <param name="newIssue">The new panel.</param>
    /// <returns>If the creation was succesfull.</returns>
    public async Task Create(Panel newPanel) =>
        await panelCollection.InsertOneAsync(newPanel);

    /// <summary>
    /// Updates the panel with the provided id.
    /// </summary>
    /// <param name="id">The identifier of the panel.</param>
    /// <param name="updatedIssue">The panel data to update.</param>
    /// <returns>If the update was succesfull.</returns>
    public async Task Update(string id, Panel updatedPanel) =>
        await panelCollection.ReplaceOneAsync(x => x.Id == id, updatedPanel);

    /// <summary>
    /// Deletes the panel with the provided id.
    /// </summary>
    /// <param name="id">The identifier of the panel.</param>
    /// <returns>If the deletion was succesfull.</returns>
    public async Task Delete(string id) =>
        await panelCollection.DeleteOneAsync(x => x.Id == id);
}