
using Microsoft.AspNetCore.Mvc;
using PocApi.Controllers;
using PocApi.Interfaces;
using PocApi.Models;

namespace PocApi.UnitTests.Controllers
{
    /// <summary>
    /// Test class for the panel controller.
    /// </summary>
    public class PanelControllerTest
    {
        [Fact]
        public async void GetSinglePanel()
        {
            // Arrange
            var testId = "1";
            var mockPanelRepo = new Mock<IPanelService>();
            var mockPageRepo = new Mock<IPageService>();            
            mockPanelRepo.Setup(repo => repo.GetWithId(testId))
                .ReturnsAsync(GetTestPanel(testId, testId));
            var controller = new PanelController(mockPanelRepo.Object, mockPageRepo.Object);

            // Act
            var result = await controller.GetById(testId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Panel>>(result);
            var model = Assert.IsAssignableFrom<Panel>(actionResult.Value);
            Assert.Equal(testId, model.Id);
        }

        [Fact]
        public async void GetPanelsForPage()
        {
            // Arrange
            var testId = "1";
            var mockPanelRepo = new Mock<IPanelService>();
            var mockPageRepo = new Mock<IPageService>();
            mockPanelRepo.Setup(repo => repo.GetAllForPage(testId))
                .ReturnsAsync(GetTestPanelsForPage(testId));
            var controller = new PanelController(mockPanelRepo.Object, mockPageRepo.Object);

            // Act
            var result = await controller.GetByPageId(testId);

            // Assert
            var listResult = Assert.IsType<List<Panel>>(result);
            Assert.True(listResult.All(x => x.PageId == testId));
        }

        [Fact]
        public async void CreatePanelForPage()
        {
            // Arrange
            var pageId = "1";
            var testId = "1";
            var newPanelTest = GetTestPanel(testId, null);
            var mockPanelRepo = new Mock<IPanelService>();
            var mockPageRepo = new Mock<IPageService>();
            mockPanelRepo.Setup(repo => repo.Create(newPanelTest))
                .Returns(Task.CompletedTask);
            mockPageRepo.Setup(repo => repo.GetWithId(pageId))
                .ReturnsAsync(new Page { Id = pageId });
            var controller = new PanelController(mockPanelRepo.Object, mockPageRepo.Object);

            // Act
            var result = await controller.Post(pageId, newPanelTest);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var model = Assert.IsType<Panel>(actionResult.Value);
            Assert.Equal(pageId, model.PageId);
        }

        [Fact]
        public async void CreatePanelForNonExcistingPage_BadRequest()
        {
            // Arrange
            var testId = "1";
            var nonExcistingPageId = "1";
            var newPanelTest = GetTestPanel(testId, null);
            var mockPanelRepo = new Mock<IPanelService>();
            var mockPageRepo = new Mock<IPageService>();
            mockPageRepo.Setup(repo => repo.GetWithId(nonExcistingPageId))
                .ReturnsAsync(null as Page);
            var controller = new PanelController(mockPanelRepo.Object, mockPageRepo.Object);

            // Act
            var result = await controller.Post(nonExcistingPageId, newPanelTest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void UpdatePanel()
        {
            // Arrange
            var excistingPanelId = "1";
            var excistingPanelTest = GetTestPanel(excistingPanelId, null);
            var mockPanelRepo = new Mock<IPanelService>();
            var mockPageRepo = new Mock<IPageService>();
            mockPanelRepo.Setup(repo => repo.GetWithId(excistingPanelId))
                .ReturnsAsync(GetTestPanel(excistingPanelId, null));
            mockPanelRepo.Setup(repo => repo.Update(excistingPanelId, excistingPanelTest))
               .Returns(Task.CompletedTask);
            
            var controller = new PanelController(mockPanelRepo.Object, mockPageRepo.Object);

            // Act
            var result = await controller.Update(excistingPanelId, excistingPanelTest);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void UpdatePage_NotFound()
        {
            // Arrange
            var nonExcistingPanelId = "1";
            var nonExcistingPanelTest = GetTestPanel(nonExcistingPanelId, null);
            var mockPanelRepo = new Mock<IPanelService>();
            var mockPageRepo = new Mock<IPageService>();
            mockPanelRepo.Setup(repo => repo.GetWithId(nonExcistingPanelId))
              .ReturnsAsync(null as Panel);
            var controller = new PanelController(mockPanelRepo.Object, mockPageRepo.Object);

            // Act
            var result = await controller.Update(nonExcistingPanelId, nonExcistingPanelTest);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void DeletePage()
        {
            // Arrange
            var excistingPanelId = "1";
            var mockPanelRepo = new Mock<IPanelService>();
            var mockPageRepo = new Mock<IPageService>();
            mockPanelRepo.Setup(repo => repo.Delete(excistingPanelId))
               .Returns(Task.CompletedTask);
            mockPanelRepo.Setup(repo => repo.GetWithId(excistingPanelId))
               .ReturnsAsync(GetTestPanel(excistingPanelId, null));
            var controller = new PanelController(mockPanelRepo.Object, mockPageRepo.Object);

            // Act
            var result = await controller.Delete(excistingPanelId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void DeleteIssue_NotFound()
        {
            // Arrange
            var nonExcistingPanelId = "1";
            var mockPanelRepo = new Mock<IPanelService>();
            var mockPageRepo = new Mock<IPageService>();
            mockPanelRepo.Setup(repo => repo.GetWithId(nonExcistingPanelId))
              .ReturnsAsync(null as Panel);
            var controller = new PanelController(mockPanelRepo.Object, mockPageRepo.Object);

            // Act
            var result = await controller.Delete(nonExcistingPanelId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        #region private methods
        /// <summary>
        /// Gets a list of test panels for a give page.
        /// </summary>
        /// <param name="issueId">The page identifier.</param>
        /// <returns>A list of panels.</returns>
        private List<Panel> GetTestPanelsForPage(string pageId)
        {
            var panels = new List<Panel>();

            for (int i = 0; i < 10; i++)
            {
                panels.Add(new Panel
                {
                    Id = $"id {i}",
                    PageId = pageId,
                    PanelOrder = i,
                    PanelDescription = $"Test {i}",
                });
            }
            return panels;
        }


        /// <summary>
        /// Gets a test panel.
        /// </summary>
        /// <param name="panelId">The panel identifier.</param>
        /// <param name="panelId">The page identifier.</param>
        /// <returns>A panel with the specified information.</returns>
        private Panel GetTestPanel(string panelId, string? pageId)
        {
            var panel = new Panel
            {
                Id = panelId,
                PageId = pageId,
                PanelOrder = 1,
                PanelDescription= "Test",
            };

            return panel;
        }
        #endregion
    }
}
