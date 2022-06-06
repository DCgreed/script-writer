using PocApi.Models;

namespace PocApi.Interfaces
{
    public interface IIssueService
    {
        /// <summary>
        /// Gets all available issues for a specified comic.
        /// </summary>
        /// <returns>A list of issues.</returns>
        public Task<List<Issue>> GetAllForComic(string id);


        /// <summary>
        /// Gets the issue for the provided id.
        /// </summary>
        /// <param name="id">The identifier of the issue.</param>
        /// <returns>A issue matching the id.</returns>
        public Task<Issue?> GetWithId(string id);

        /// <summary>
        /// Creates a new issue.
        /// </summary>
        /// <param name="newIssue">The new issue.</param>
        /// <returns>The new issue.</returns>
        public Task Create(Issue newIssue);

        /// <summary>
        /// Updates the issue with the provided id.
        /// </summary>
        /// <param name="id">The identifier of the issue.</param>
        /// <param name="updatedIssue">The issue data to update.</param>
        /// <returns>If the update was succesfull.</returns>
        public Task Update(string id, Issue updatedIssue);

        /// <summary>
        /// Deletes the issue with the provided id.
        /// </summary>
        /// <param name="id">The identifier of the issue.</param>
        /// <returns>If the deletion was succesfull.</returns>
        public Task Delete(string id);
    }
}
