using PocApi.Models;
using PocApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PocApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IssueController : ControllerBase
{
    #region setup
    private readonly IIssueService issueService;
    private readonly IComicService comicService;

    /// <summary>
    /// The issue controller constructor.
    /// </summary>
    /// <param name="issueService">The issue service.</param>
    /// <param name="comicsService">The comic service.</param>
    public IssueController(IIssueService issueService, IComicService comicsService)
    {
        this.issueService = issueService;
        comicService = comicsService;
    }
    #endregion

    #region Api calls
    /// <summary>
    /// Gets the issue for specified id.
    /// </summary>
    /// <param name="id">The identifier of the issue.</param>
    /// <returns>A single issue.</returns>
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Issue>> GetById(string id)
    {
        var issue = await issueService.GetWithId(id);

        if (issue is null)
        {
            return NotFound();
        }

        return issue;
    }

    /// <summary>
    /// Gets all issues for the specified comic id.
    /// </summary>
    /// <param name="id">The identifier of the comic.</param>
    /// <returns>List of issues.</returns>
    [HttpGet("comic/{comicId:length(24)}")]
    public async Task<List<Issue>> GetByComicId(string comicId)
    {
        var issues = await issueService.GetAllForComic(comicId);

        return issues;
    }

    /// <summary>
    /// Creates a new isssue with the given information.
    /// </summary>
    /// <param name="newIssue">The new issue.</param>
    /// <returns>The newly created issue.</returns>
    [HttpPost("{comicId:length(24)}")]
    public async Task<IActionResult> Post(string comicId, Issue newIssue)
    {
        var comic = await comicService.GetWithId(comicId);

        if (comic is null)
        {
            return BadRequest("Comic not found");
        }

        newIssue.ComicId = comic.Id;

        await issueService.Create(newIssue);

        return CreatedAtAction(nameof(GetById), new { id = newIssue.Id }, newIssue);
    }

    /// <summary>
    /// Updates the issue for the specified id.
    /// </summary>
    /// <param name="id">The issue identifier.</param>
    /// <param name="updatedComic">The changed issue.</param>
    /// <returns>No contend if succesfull.Else not found.</returns>
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Issue updatedIssue)
    {
        var issue = await issueService.GetWithId(id);

        if (issue is null)
        {
            return NotFound();
        }

        updatedIssue.Id = issue.Id;

        await issueService.Update(id, updatedIssue);

        return NoContent();
    }

    /// <summary>
    /// Deletes the issue with the specified id.
    /// </summary>
    /// <param name="id">The issue identifier.</param>
    /// <returns>No contend if succesfull.Else not found.</returns>
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var issue = await issueService.GetWithId(id);

        if (issue is null)
        {
            return NotFound();
        }

        await issueService.Delete(id);

        return NoContent();
    }
    #endregion
}