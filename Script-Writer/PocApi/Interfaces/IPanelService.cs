using PocApi.Models;

namespace PocApi.Interfaces
{
    public interface IPanelService
    {
        /// <summary>
        /// Gets all available panels for a specified page.
        /// </summary>
        /// <param name="pageId">The identifier of the page.</param>
        /// <returns>A list of panels.</returns>
        public Task<List<Panel>> GetAllForPage(string pageId);


        /// <summary>
        /// Gets the panel for the provided id.
        /// </summary>
        /// <param name="id">The identifier of the panel.</param>
        /// <returns>A panel matching the id.</returns>
        public Task<Panel?> GetWithId(string id);

        /// <summary>
        /// Creates a new panel.
        /// </summary>
        /// <param name="newPanel">The new panel.</param>
        /// <returns>The new panel.</returns>
        public Task Create(Panel newPanel);

        /// <summary>
        /// Updates the panel with the provided id.
        /// </summary>
        /// <param name="id">The identifier of the panel.</param>
        /// <param name="updatedPanel">The panel data to update.</param>
        /// <returns>If the update was succesfull.</returns>
        public Task Update(string id, Panel updatedPanel);

        /// <summary>
        /// Deletes the panel with the provided id.
        /// </summary>
        /// <param name="id">The identifier of the panel.</param>
        /// <returns>If the deletion was succesfull.</returns>
        public Task Delete(string id);
    }
}
