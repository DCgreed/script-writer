
using Microsoft.AspNetCore.Mvc;
using PocApi.Controllers;
using PocApi.Interfaces;
using PocApi.Models;

namespace PocApi.UnitTests.Controllers
{
    /// <summary>
    /// Test class for the dialogue controller.
    /// </summary>
    public class DialogueControllerTest
    {
        [Fact]
        public async void GetSingleDialogue()
        {
            // Arrange
            var testId = "1";
            var mockDialogueRepo = new Mock<IDialogueService>();
            var mockPanelRepo = new Mock<IPanelService>();
            mockDialogueRepo.Setup(repo => repo.GetWithId(testId))
                .ReturnsAsync(GetTestDialogue(testId, testId));
            var controller = new DialogueController(mockDialogueRepo.Object ,mockPanelRepo.Object);

            // Act
            var result = await controller.GetById(testId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Dialogue>>(result);
            var model = Assert.IsAssignableFrom<Dialogue>(actionResult.Value);
            Assert.Equal(testId, model.Id);
        }

        [Fact]
        public async void GetDialoguesForPanel()
        {
            // Arrange
            var testId = "1";
            var mockDialogueRepo = new Mock<IDialogueService>();
            var mockPanelRepo = new Mock<IPanelService>();
            mockDialogueRepo.Setup(repo => repo.GetAllForPanel(testId))
                .ReturnsAsync(GetTestDialoguesForPanel(testId));
            var controller = new DialogueController(mockDialogueRepo.Object, mockPanelRepo.Object);

            // Act
            var result = await controller.GetByPanelId(testId);

            // Assert
            var listResult = Assert.IsType<List<Dialogue>>(result);
            Assert.True(listResult.All(x => x.PanelId == testId));
        }

        [Fact]
        public async void CreateDialogueForPanel()
        {
            // Arrange
            var panelId = "1";
            var testId = "1";
            var newPanelTest = GetTestDialogue(testId, null);
            var mockDialogueRepo = new Mock<IDialogueService>();
            var mockPanelRepo = new Mock<IPanelService>();
            mockPanelRepo.Setup(repo => repo.GetWithId(panelId))
                .ReturnsAsync(new Panel { Id = panelId });
            mockDialogueRepo.Setup(repo => repo.Create(newPanelTest))
                .Returns(Task.CompletedTask);            
            var controller = new DialogueController(mockDialogueRepo.Object, mockPanelRepo.Object);

            // Act
            var result = await controller.Post(panelId, newPanelTest);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var model = Assert.IsType<Dialogue>(actionResult.Value);
            Assert.Equal(panelId, model.PanelId);
        }

        [Fact]
        public async void CreateDialogueForNonExcistingPanel_BadRequest()
        {
            // Arrange
            var testId = "1";
            var nonExcistingPageId = "1";
            var newPanelTest = GetTestDialogue(testId, null);
            var mockDialogueRepo = new Mock<IDialogueService>();
            var mockPanelRepo = new Mock<IPanelService>();
            mockPanelRepo.Setup(repo => repo.GetWithId(nonExcistingPageId))
                .ReturnsAsync(null as Panel);
            var controller = new DialogueController(mockDialogueRepo.Object, mockPanelRepo.Object);

            // Act
            var result = await controller.Post(nonExcistingPageId, newPanelTest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void UpdatePanel()
        {
            // Arrange
            var excistingDialogueId = "1";
            var excistingDialogueTest = GetTestDialogue(excistingDialogueId, null);
            var mockDialogueRepo = new Mock<IDialogueService>();
            var mockPanelRepo = new Mock<IPanelService>();
            mockDialogueRepo.Setup(repo => repo.GetWithId(excistingDialogueId))
                .ReturnsAsync(GetTestDialogue(excistingDialogueId, null));
            mockDialogueRepo.Setup(repo => repo.Update(excistingDialogueId, excistingDialogueTest))
               .Returns(Task.CompletedTask);
            var controller = new DialogueController(mockDialogueRepo.Object, mockPanelRepo.Object);

            // Act
            var result = await controller.Update(excistingDialogueId, excistingDialogueTest);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void UpdatePage_NotFound()
        {
            // Arrange
            var nonExcistingDialogueId = "1";
            var nonExcistingDialogueTest = GetTestDialogue(nonExcistingDialogueId, null);
            var mockDialogueRepo = new Mock<IDialogueService>();
            var mockPanelRepo = new Mock<IPanelService>();
            mockDialogueRepo.Setup(repo => repo.GetWithId(nonExcistingDialogueId))
              .ReturnsAsync(null as Dialogue);
            var controller = new DialogueController(mockDialogueRepo.Object, mockPanelRepo.Object);

            // Act
            var result = await controller.Update(nonExcistingDialogueId, nonExcistingDialogueTest);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void DeletePage()
        {
            // Arrange
            var excistingDialogueId = "1";
            var mockDialogueRepo = new Mock<IDialogueService>();
            var mockPanelRepo = new Mock<IPanelService>();
            mockDialogueRepo.Setup(repo => repo.Delete(excistingDialogueId))
               .Returns(Task.CompletedTask);
            mockDialogueRepo.Setup(repo => repo.GetWithId(excistingDialogueId))
               .ReturnsAsync(GetTestDialogue(excistingDialogueId, null));
            var controller = new DialogueController(mockDialogueRepo.Object, mockPanelRepo.Object);

            // Act
            var result = await controller.Delete(excistingDialogueId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void DeleteIssue_NotFound()
        {
            // Arrange
            var nonExcistingDialogueId = "1";
            var mockDialogueRepo = new Mock<IDialogueService>();
            var mockPanelRepo = new Mock<IPanelService>();
            mockPanelRepo.Setup(repo => repo.GetWithId(nonExcistingDialogueId))
              .ReturnsAsync(null as Panel);
            var controller = new DialogueController(mockDialogueRepo.Object, mockPanelRepo.Object);

            // Act
            var result = await controller.Delete(nonExcistingDialogueId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        #region private methods
        /// <summary>
        /// Gets a list of test dialouges for a given panel.
        /// </summary>
        /// <param name="panelId">The panel identifier.</param>
        /// <returns>A list of dialogues.</returns>
        private List<Dialogue> GetTestDialoguesForPanel(string panelId)
        {
            var dialouges = new List<Dialogue>();

            for (int i = 0; i < 10; i++)
            {
                dialouges.Add(new Dialogue
                {
                    Id = $"id {i}",
                    PanelId = panelId,
                    Order = i,
                    Line = $"Text {i}"
                });
            }
            return dialouges;
        }


        /// <summary>
        /// Gets a test dialogue.
        /// </summary>
        /// <param name="dialougeId">The dialogue identifier.</param>
        /// <param name="panelId">The panel identifier.</param>
        /// <returns>A dialogue with the specified information.</returns>
        private Dialogue GetTestDialogue(string dialougeId, string? panelId)
        {
            var dialogue = new Dialogue
            {
                Id = dialougeId,
                PanelId = panelId,
                Order = 1,
                Line = "Text"
            };

            return dialogue;
        }
        #endregion
    }
}
