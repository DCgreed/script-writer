using PocApi.Models;

namespace PocApi.Interfaces
{
    public interface IDialogueService
    {
        /// <summary>
        /// Gets the dialogue for the provided id.
        /// </summary>
        /// <param name="id">The identifier of the dialogue.</param>
        /// <returns>A dialogue matching the id.</returns>
        public Task<Dialogue?> GetWithId(string id);

        /// <summary>
        /// Gets all available dialogues for a specified panel.
        /// </summary>
        /// <param name="panelId">The identifier of the panel.</param>
        /// <returns>A list of dialogues.</returns>
        public Task<List<Dialogue>> GetAllForPanel(string panelId);

        /// <summary>
        /// Creates a new dialogue.
        /// </summary>
        /// <param name="newDialogue">The new dialogue.</param>
        /// <returns>The new dialogue.</returns>
        public Task Create(Dialogue newDialogue);

        /// <summary>
        /// Updates the dialogue with the provided id.
        /// </summary>
        /// <param name="id">The identifier of the dialogue.</param>
        /// <param name="updatedDialogue">The dialogue data to update.</param>
        /// <returns>If the update was succesfull.</returns>
        public Task Update(string id, Dialogue updatedDialogue);

        /// <summary>
        /// Deletes the dialogue with the provided id.
        /// </summary>
        /// <param name="id">The identifier of the dialogue.</param>
        /// <returns>If the deletion was succesfull.</returns>
        public Task Delete(string id);
    }
}
