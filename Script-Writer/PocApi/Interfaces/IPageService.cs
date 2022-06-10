using PocApi.Models;

namespace PocApi.Interfaces
{
    public interface IPageService
    {
        /// <summary>
        /// Gets all available pages for a specified issue.
        /// </summary>
        /// <param name="issueId">The identifier of the issue.</param>
        /// <returns>A list of pages.</returns>
        public Task<List<Page>> GetAllForIssue(string issueId);


        /// <summary>
        /// Gets the page for the provided id.
        /// </summary>
        /// <param name="id">The identifier of the page.</param>
        /// <returns>A page matching the id.</returns>
        public Task<Page?> GetWithId(string id);

        /// <summary>
        /// Creates a new page.
        /// </summary>
        /// <param name="newPage">The new page.</param>
        /// <returns>The new page.</returns>
        public Task Create(Page newPage);

        /// <summary>
        /// Updates the page with the provided id.
        /// </summary>
        /// <param name="id">The identifier of the page.</param>
        /// <param name="updatedPage">The page data to update.</param>
        /// <returns>If the update was succesfull.</returns>
        public Task Update(string id, Page updatedPage);

        /// <summary>
        /// Deletes the page with the provided id.
        /// </summary>
        /// <param name="id">The identifier of the page.</param>
        /// <returns>If the deletion was succesfull.</returns>
        public Task Delete(string id);
    }
}
