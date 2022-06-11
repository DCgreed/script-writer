using PocApi.Models;
using PocApi.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace PocApi.Services;

public class DialogueService : IDialogueService
{
    private readonly IMongoCollection<Dialogue> dialogueCollection;

    public DialogueService(
        IOptions<ComicConnectionSettings> bookStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            bookStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            bookStoreDatabaseSettings.Value.DatabaseName);

        dialogueCollection = mongoDatabase.GetCollection<Dialogue>("dialogue");
    }

    /// <summary>
    /// Gets the dialogue for the provided id.
    /// </summary>
    /// <param name="id">The identifier of the dialouge.</param>
    /// <returns>A dialogue matching the id.</returns>
    public async Task<Dialogue?> GetWithId(string id) =>
        await dialogueCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    /// <summary>
    /// Gets all available dialouges for a specified panel.
    /// </summary>
    /// <returns>A list of dialogues.</returns>
    public async Task<List<Dialogue>> GetAllForPanel(string panelId) =>
        await dialogueCollection.Find(x => x.PanelId == panelId).ToListAsync();    

    /// <summary>
    /// Creates a new dialogue.
    /// </summary>
    /// <param name="newDialouge">The new dialogue.</param>
    /// <returns>If the creation was succesfull.</returns>
    public async Task Create(Dialogue newDialogue) =>
        await dialogueCollection.InsertOneAsync(newDialogue);

    /// <summary>
    /// Updates the dialogue with the provided id.
    /// </summary>
    /// <param name="id">The identifier of the dialogue.</param>
    /// <param name="updatedDialogue">The dialogue data to update.</param>
    /// <returns>If the update was succesfull.</returns>
    public async Task Update(string id, Dialogue updatedDialogue) =>
        await dialogueCollection.ReplaceOneAsync(x => x.Id == id, updatedDialogue);

    /// <summary>
    /// Deletes the dialouge with the provided id.
    /// </summary>
    /// <param name="id">The identifier of the dialogue.</param>
    /// <returns>If the deletion was succesfull.</returns>
    public async Task Delete(string id) =>
        await dialogueCollection.DeleteOneAsync(x => x.Id == id);
}