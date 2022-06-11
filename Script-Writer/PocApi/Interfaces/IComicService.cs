using PocApi.Models;

namespace PocApi.Interfaces
{
    public interface IComicService
    {
        /// <summary>
        /// Gets all available comics.
        /// </summary>
        /// <returns>A list of comics.</returns>
        public Task<List<Comic>> GetAll();

        /// <summary>
        /// Gets the comic for the provided id.
        /// </summary>
        /// <param name="id">The identifier of the comic.</param>
        /// <returns>A comic matching the id.</returns>
        public Task<Comic?> GetWithId(string id);

        /// <summary>
        /// Creates a new comic.
        /// </summary>
        /// <param name="newComic">The new comic.</param>
        /// <returns>If the create was succesfull.</returns>
        public Task Create(Comic newComic);

        /// <summary>
        /// Updates the comic with the provided id.
        /// </summary>
        /// <param name="id">The identifier of the comic.</param>
        /// <param name="updatedComic">The comic data to update.</param>
        /// <returns>If the update was succesfull.</returns>
        public Task Update(string id, Comic updatedComic);

        /// <summary>
        /// Deletes the comic with the provided id.
        /// </summary>
        /// <param name="id">The identifier of the comic.</param>
        /// <returns>If the deletion was succesfull.</returns>
        public Task Delete(string id);
    }
}
