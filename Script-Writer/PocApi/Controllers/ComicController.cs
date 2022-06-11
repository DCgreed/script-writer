using PocApi.Models;
using PocApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PocApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComicController : ControllerBase
{
    private readonly IComicService comicService;

    public ComicController(IComicService comicsService) =>
        this.comicService = comicsService;

    /// <summary>
    /// Gets all comics.
    /// </summary>
    /// <returns>A collection of comics</returns>
    [HttpGet]
    public async Task<List<Comic>> Get() =>
        await comicService.GetAll();

    
    /// <summary>
    /// Gets the comic for specified id.
    /// </summary>
    /// <param name="id">The identifier of the comic.</param>
    /// <returns>A single comic.</returns>
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Comic>> GetById(string id)
    {
        var Comic = await comicService.GetWithId(id);

        if (Comic is null)
        {
            return NotFound();
        }

        return Comic;
    }

    /// <summary>
    /// Creates a new comic with the given information.
    /// </summary>
    /// <param name="newComic">The new comic.</param>
    /// <returns>The newly created comic.</returns>
    [HttpPost]
    public async Task<IActionResult> Post(Comic newComic)
    {
        await comicService.Create(newComic);

        return CreatedAtAction(nameof(Get), new { id = newComic.Id }, newComic);
    }

    /// <summary>
    /// Updates the comic for the specified id.
    /// </summary>
    /// <param name="id">The comic identifier.</param>
    /// <param name="updatedComic">The changed comic.</param>
    /// <returns>No contend if succesfull.Else not found.</returns>
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Comic updatedComic)
    {
        var Comic = await comicService.GetWithId(id);

        if (Comic is null)
        {
            return NotFound();
        }

        updatedComic.Id = Comic.Id;

        await comicService.Update(id, updatedComic);

        return NoContent();
    }

    /// <summary>
    /// Deletes the comic with the specified id.
    /// </summary>
    /// <param name="id">The comic identifier.</param>
    /// <returns>No contend if succesfull.Else not found.</returns>
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var Comic = await comicService.GetWithId(id);

        if (Comic is null)
        {
            return NotFound();
        }

        await comicService.Delete(id);

        return NoContent();
    }
}