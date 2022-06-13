using PocApi.Models;
using PocApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PocApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DialogueController : ControllerBase
{
    #region setup
    private readonly IDialogueService dialogueService;
    private readonly IPanelService panelService;
    

    /// <summary>
    /// The dialogue controller constructor.
    /// </summary>
    /// <param name="dialogueService">The dialogue service.</param>
    /// <param name="panelService">The panel service.</param>
    public DialogueController(IDialogueService dialogueService ,IPanelService panelService)
    {
        this.dialogueService = dialogueService;
        this.panelService = panelService;
    }
    #endregion

    #region Api calls
    /// <summary>
    /// Gets the dialogue for the specified id.
    /// </summary>
    /// <param name="id">The identifier of the dialogue.</param>
    /// <returns>A single dialogue.</returns>
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Dialogue>> GetById(string id)
    {
        var dialogue = await dialogueService.GetWithId(id);

        if (dialogue is null)
        {
            return NotFound();
        }

        return dialogue;
    }

    /// <summary>
    /// Gets all dialogues for the specified panel id.
    /// </summary>
    /// <param name="id">The identifier of the panel.</param>
    /// <returns>List of dialogues.</returns>
    [HttpGet("panel/{panelId:length(24)}")]
    public async Task<List<Dialogue>> GetByPanelId(string panelId)
    {
        var dialogues = await dialogueService.GetAllForPanel(panelId);

        return dialogues;
    }

    /// <summary>
    /// Creates a new dialogue with the given information.
    /// </summary>
    /// <param name="panelId">The identifier of the panel.</param>
    /// <param name="newDialogue">The new dialogue.</param>
    /// <returns>The newly created dialogue.</returns>
    [HttpPost("{panelId:length(24)}")]
    public async Task<IActionResult> Post(string panelId, Dialogue newDialogue)
    {
        var panel = await panelService.GetWithId(panelId);

        if (panel is null)
        {
            return BadRequest("Panel not found");
        }

        newDialogue.PanelId = panel.Id;

        await dialogueService.Create(newDialogue);

        return CreatedAtAction(nameof(GetById), new { id = newDialogue.Id }, newDialogue);
    }

    /// <summary>
    /// Updates the dialogue for the specified id.
    /// </summary>
    /// <param name="id">The dialogue identifier.</param>
    /// <param name="updatedDialogue">The changed dialogue.</param>
    /// <returns>No contend if succesfull.Else not found.</returns>
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Dialogue updatedDialogue)
    {
        var dialogue = await dialogueService.GetWithId(id);

        if (dialogue is null)
        {
            return NotFound();
        }

        updatedDialogue.Id = dialogue.Id;

        await dialogueService.Update(id, updatedDialogue);

        return NoContent();
    }

    /// <summary>
    /// Deletes the dialogue with the specified id.
    /// </summary>
    /// <param name="id">The dialogue identifier.</param>
    /// <returns>No contend if succesfull.Else not found.</returns>
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var dialogue = await dialogueService.GetWithId(id);

        if (dialogue is null)
        {
            return NotFound();
        }

        await dialogueService.Delete(id);

        return NoContent();
    }
    #endregion
}