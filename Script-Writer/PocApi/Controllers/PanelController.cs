using PocApi.Models;
using PocApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PocApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PanelController : ControllerBase
{
    #region setup
    private readonly IPanelService panelService;
    private readonly IPageService pageService;

    /// <summary>
    /// The panel controller constructor.
    /// </summary>
    /// <param name="panelService">The panel service.</param>
    /// <param name="pageService">The page service.</param>
    public PanelController(IPanelService panelService, IPageService pageService)
    {
        this.panelService = panelService;
        this.pageService = pageService;
        
    }
    #endregion

    #region Api calls
    /// <summary>
    /// Gets the panel for specified id.
    /// </summary>
    /// <param name="id">The identifier of the panel.</param>
    /// <returns>A single panel.</returns>
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Panel>> GetById(string id)
    {
        var panel = await panelService.GetWithId(id);

        if (panel is null)
        {
            return NotFound();
        }

        return panel;
    }

    /// <summary>
    /// Gets all panels for the specified page id.
    /// </summary>
    /// <param name="id">The identifier of the page.</param>
    /// <returns>List of panels.</returns>
    [HttpGet("page/{pageId:length(24)}")]
    public async Task<List<Panel>> GetByPageId(string pageId)
    {
        var panels = await panelService.GetAllForPage(pageId);

        return panels;
    }

    /// <summary>
    /// Creates a new panel with the given information.
    /// </summary>
    /// <param name="pageId">The identifier of the page.</param>
    /// <param name="newPanel">The new panel.</param>
    /// <returns>The newly created panel.</returns>
    [HttpPost("{pageId:length(24)}")]
    public async Task<IActionResult> Post(string pageId, Panel newPanel)
    {
        var page = await pageService.GetWithId(pageId);

        if (page is null)
        {
            return BadRequest("Page not found");
        }

        newPanel.PageId = page.Id;

        await panelService.Create(newPanel);

        return CreatedAtAction(nameof(GetById), new { id = newPanel.Id }, newPanel);
    }

    /// <summary>
    /// Updates the panel for the specified id.
    /// </summary>
    /// <param name="id">The panel identifier.</param>
    /// <param name="updatedPanel">The changed panel.</param>
    /// <returns>No contend if succesfull.Else not found.</returns>
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Panel updatedPanel)
    {
        var panel = await panelService.GetWithId(id);

        if (panel is null)
        {
            return NotFound();
        }

        updatedPanel.Id = panel.Id;

        await panelService.Update(id, updatedPanel);

        return NoContent();
    }

    /// <summary>
    /// Deletes the panel with the specified id.
    /// </summary>
    /// <param name="id">The panel identifier.</param>
    /// <returns>No contend if succesfull.Else not found.</returns>
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var panel = await panelService.GetWithId(id);

        if (panel is null)
        {
            return NotFound();
        }

        await panelService.Delete(id);

        return NoContent();
    }
    #endregion
}