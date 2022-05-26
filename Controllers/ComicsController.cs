using PocApi.Models;
using PocApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace PocApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComicsController : ControllerBase
{
    private readonly ComicsService _ComicsService;

    public ComicsController(ComicsService ComicsService) =>
        _ComicsService = ComicsService;

    [HttpGet]
    public async Task<List<Comic>> Get() =>
        await _ComicsService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Comic>> Get(string id)
    {
        var Comic = await _ComicsService.GetAsync(id);

        if (Comic is null)
        {
            return NotFound();
        }

        return Comic;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Comic newComic)
    {
        await _ComicsService.CreateAsync(newComic);

        return CreatedAtAction(nameof(Get), new { id = newComic.Id }, newComic);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Comic updatedComic)
    {
        var Comic = await _ComicsService.GetAsync(id);

        if (Comic is null)
        {
            return NotFound();
        }

        updatedComic.Id = Comic.Id;

        await _ComicsService.UpdateAsync(id, updatedComic);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var Comic = await _ComicsService.GetAsync(id);

        if (Comic is null)
        {
            return NotFound();
        }

        await _ComicsService.RemoveAsync(id);

        return NoContent();
    }
}