using PocApi.Models;
using PocApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PocApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActorController : ControllerBase
{
    #region setup
    private readonly IActorService actorService;
    private readonly IComicService comicService;

    /// <summary>
    /// The actor controller constructor.
    /// </summary>
    /// <param name="actorService">The actor service.</param>
    /// <param name="comicService">The comic service.</param>
    public ActorController(IActorService actorService, IComicService comicService)
    {
        this.actorService = actorService;
        this.comicService = comicService;
        
    }
    #endregion

    #region Api calls
    /// <summary>
    /// Gets the actor for the specified id.
    /// </summary>
    /// <param name="id">The identifier of the actor.</param>
    /// <returns>A single actor.</returns>
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Actor>> GetById(string id)
    {
        var actor = await actorService.GetWithId(id);

        if (actor is null)
        {
            return NotFound();
        }

        return actor;
    }

    /// <summary>
    /// Gets all actors for for the specified comic id.
    /// </summary>
    /// <param name="comicId">The identifier of the comic.</param>
    /// <returns>List of actors.</returns>
    [HttpGet("comic/{comicId:length(24)}")]
    public async Task<ActionResult<List<Actor>>> GetAllForComic(string comicId)
    {
        var comic = await comicService.GetWithId(comicId);

        if (comic is null)
        {
            return BadRequest("Comic not found");
        }

        var actors = await actorService.GetAllForComic(comicId);

        return actors;
    }

    /// <summary>
    /// Creates a new actor with the given information for the specified comic id.
    /// </summary>
    /// <param name="comicId">The identifier of the comic.</param>
    /// <param name="newActor">The new actor.</param>
    /// <returns>The newly created panel.</returns>
    [HttpPost("{actorId:length(24)}")]
    public async Task<IActionResult> Post(string comicId, Actor newActor)
    {
        var comic = await comicService.GetWithId(comicId);

        if (comic is null)
        {
            return BadRequest("Comic not found");
        }

        newActor.ComicId = comic.Id;

        await actorService.Create(newActor);

        return CreatedAtAction(nameof(GetById), new { id = newActor.Id }, newActor);
    }

    /// <summary>
    /// Updates the actor for the specified id.
    /// </summary>
    /// <param name="id">The actor identifier.</param>
    /// <param name="updatedActor">The changed actor.</param>
    /// <returns>No contend if succesfull.Else not found.</returns>
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Actor updatedActor)
    {
        var actor = await actorService.GetWithId(id);

        if (actor is null)
        {
            return NotFound();
        }

        updatedActor.Id = actor.Id;

        await actorService.Update(id, updatedActor);

        return NoContent();
    }

    /// <summary>
    /// Deletes the actor with the specified id.
    /// </summary>
    /// <param name="id">The actor identifier.</param>
    /// <returns>No contend if succesfull.Else not found.</returns>
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var actor = await actorService.GetWithId(id);

        if (actor is null)
        {
            return NotFound();
        }

        await actorService.Delete(id);

        return NoContent();
    }
    #endregion
}