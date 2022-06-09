using PocApi.Models;
using PocApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PocApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PageController : ControllerBase
{
    #region setup
    private readonly IIssueService issueService;
    private readonly IPageService pageService;

    /// <summary>
    /// The page controller constructor.
    /// </summary>
    /// <param name="pageService">The page service.</param>
    /// <param name="issueService">The issue service.</param>
    public PageController(IPageService pageService, IIssueService issueService)
    {
        this.pageService = pageService;
        this.issueService = issueService;
    }
    #endregion

    #region Api calls
    /// <summary>
    /// Gets the page for specified id.
    /// </summary>
    /// <param name="id">The identifier of the page.</param>
    /// <returns>A single page.</returns>
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Page>> GetById(string id)
    {
        var page = await pageService.GetWithId(id);

        if (page is null)
        {
            return NotFound();
        }

        return page;
    }

    /// <summary>
    /// Gets all pages for the specified issue id.
    /// </summary>
    /// <param name="id">The identifier of the issue.</param>
    /// <returns>List of issues.</returns>
    [HttpGet("issue/{pageId:length(24)}")]
    public async Task<List<Page>> GetByComicId(string issueId)
    {
        var pages = await pageService.GetAllForIssue(issueId);

        return pages;
    }

    /// <summary>
    /// Creates a new page with the given information.
    /// </summary>
    /// <param name="issueId">The identifier of the issue.</param>
    /// <param name="newPage">The new page.</param>
    /// <returns>The newly created issue.</returns>
    [HttpPost("{comicId:length(24)}")]
    public async Task<IActionResult> Post(string issueId, Page newPage)
    {
        var issue = await issueService.GetWithId(issueId);

        if (issue is null)
        {
            return BadRequest("Issue not found");
        }

        newPage.IssueId = issue.Id;

        await pageService.Create(newPage);

        return CreatedAtAction(nameof(GetById), new { id = newPage.Id }, newPage);
    }

    /// <summary>
    /// Updates the page for the specified id.
    /// </summary>
    /// <param name="id">The page identifier.</param>
    /// <param name="updatedComic">The changed page.</param>
    /// <returns>No contend if succesfull.Else not found.</returns>
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Page updatedPage)
    {
        var page = await pageService.GetWithId(id);

        if (page is null)
        {
            return NotFound();
        }

        updatedPage.Id = page.Id;

        await pageService.Update(id, updatedPage);

        return NoContent();
    }

    /// <summary>
    /// Deletes the page with the specified id.
    /// </summary>
    /// <param name="id">The page identifier.</param>
    /// <returns>No contend if succesfull.Else not found.</returns>
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var page = await pageService.GetWithId(id);

        if (page is null)
        {
            return NotFound();
        }

        await pageService.Delete(id);

        return NoContent();
    }
    #endregion
}