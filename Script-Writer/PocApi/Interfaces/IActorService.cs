using PocApi.Models;

namespace PocApi.Interfaces
{
    public interface IActorService
    {
        /// <summary>
        /// Gets the actor for the provided id.
        /// </summary>
        /// <param name="id">The identifier of the actor.</param>
        /// <returns>A actor matching the id.</returns>
        public Task<Actor?> GetWithId(string id);

        /// <summary>
        /// Gets all actors for the specified comic id.
        /// </summary>
        /// <param name="comicId">The identifier of the comic.</param>
        /// <returns>A list of actors.</returns>
        public Task<List<Actor>> GetAllForComic(string comicId);

        /// <summary>
        /// Creates a new actor.
        /// </summary>
        /// <param name="newActor">The new actor.</param>
        /// <returns>The new actor.</returns>
        public Task Create(Actor newActor);

        /// <summary>
        /// Updates the actor with the provided id.
        /// </summary>
        /// <param name="id">The identifier of the actor.</param>
        /// <param name="updatedDialogue">The actor data to update.</param>
        /// <returns>If the update was succesfull.</returns>
        public Task Update(string id, Actor updatedActor);

        /// <summary>
        /// Deletes the actor with the provided id.
        /// </summary>
        /// <param name="id">The identifier of the actor.</param>
        /// <returns>If the deletion was succesfull.</returns>
        public Task Delete(string id);
    }
}
