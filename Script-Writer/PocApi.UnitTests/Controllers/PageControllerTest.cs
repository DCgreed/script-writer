
using Microsoft.AspNetCore.Mvc;
using PocApi.Controllers;
using PocApi.Interfaces;
using PocApi.Models;

namespace PocApi.UnitTests.Controllers
{
    /// <summary>
    /// Test class for the page controller.
    /// </summary>
    public class PageControllerTest
    {
        [Fact]
        public async void GetSinglePage()
        {
            // Arrange
            var testId = "1";
            var mockPageRepo = new Mock<IPageService>();
            var mockIssueRepo = new Mock<IIssueService>();
            mockPageRepo.Setup(repo => repo.GetWithId(testId))
                .ReturnsAsync(GetTestPage(testId, testId));
            var controller = new PageController(mockPageRepo.Object, mockIssueRepo.Object);

            // Act
            var result = await controller.GetById(testId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Page>>(result);
            var model = Assert.IsAssignableFrom<Page>(actionResult.Value);
            Assert.Equal(testId, model.Id);
        }

        [Fact]
        public async void GetPageForIssue()
        {
            // Arrange
            var testId = "1";
            var mockPageRepo = new Mock<IPageService>();
            var mockIssueRepo = new Mock<IIssueService>();
            mockPageRepo.Setup(repo => repo.GetAllForIssue(testId))
                .ReturnsAsync(GetTestPagesForIssue(testId));
            var controller = new PageController(mockPageRepo.Object, mockIssueRepo.Object);

            // Act
            var result = await controller.GetByComicId(testId);

            // Assert
            var listResult = Assert.IsType<List<Page>>(result);
            Assert.True(listResult.All(x => x.IssueId == testId));
        }

        [Fact]
        public async void CreatePageForIssue()
        {
            // Arrange
            var issueId = "1";
            var newPageTest = GetTestPage("page 1", null);
            var mockPageRepo = new Mock<IPageService>();
            var mockIssueRepo = new Mock<IIssueService>();
            mockPageRepo.Setup(repo => repo.Create(newPageTest))
                .Returns(Task.CompletedTask);
            mockIssueRepo.Setup(repo => repo.GetWithId(issueId))
                .ReturnsAsync(new Issue { Id = issueId });
            var controller = new PageController(mockPageRepo.Object, mockIssueRepo.Object);

            // Act
            var result = await controller.Post(issueId, newPageTest);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var model = Assert.IsType<Page>(actionResult.Value);
            Assert.Equal(issueId, model.IssueId);
        }

        [Fact]
        public async void CreatePageForNonExcistingIssue_BadRequest()
        {
            // Arrange
            var nonExcistingIssueId = "1";
            var newPageTest = GetTestPage("issue 1", null);
            var mockPageRepo = new Mock<IPageService>();
            var mockIssueRepo = new Mock<IIssueService>();
            var controller = new PageController(mockPageRepo.Object, mockIssueRepo.Object);

            // Act
            var result = await controller.Post(nonExcistingIssueId, newPageTest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void UpdatePage()
        {
            // Arrange
            var excistingPageId = "1";
            var excistingPageTest = GetTestPage(excistingPageId, null);
            var mockPageRepo = new Mock<IPageService>();
            var mockIssueRepo = new Mock<IIssueService>();
            mockPageRepo.Setup(repo => repo.Update(excistingPageId, excistingPageTest))
               .Returns(Task.CompletedTask);
            mockPageRepo.Setup(repo => repo.GetWithId(excistingPageId))
                .ReturnsAsync(GetTestPage(excistingPageId, null));
            var controller = new PageController(mockPageRepo.Object, mockIssueRepo.Object);

            // Act
            var result = await controller.Update(excistingPageId, excistingPageTest);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void UpdatePage_NotFound()
        {
            // Arrange
            var nonExcistingPageId = "1";
            var nonExcistingPageTest = GetTestPage(nonExcistingPageId, null);
            var mockPageRepo = new Mock<IPageService>();
            var mockIssueRepo = new Mock<IIssueService>();
            mockPageRepo.Setup(repo => repo.GetWithId(nonExcistingPageId))
              .ReturnsAsync(null as Page);
            var controller = new PageController(mockPageRepo.Object, mockIssueRepo.Object);

            // Act
            var result = await controller.Update(nonExcistingPageId, nonExcistingPageTest);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void DeletePage()
        {
            // Arrange
            var excistingPageId = "1";
            var mockPageRepo = new Mock<IPageService>();
            var mockIssueRepo = new Mock<IIssueService>();
            mockPageRepo.Setup(repo => repo.Delete(excistingPageId))
               .Returns(Task.CompletedTask);
            mockPageRepo.Setup(repo => repo.GetWithId(excistingPageId))
               .ReturnsAsync(GetTestPage(excistingPageId, null));
            var controller = new PageController(mockPageRepo.Object, mockIssueRepo.Object);

            // Act
            var result = await controller.Delete(excistingPageId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void DeleteIssue_NotFound()
        {
            // Arrange
            var nonExcistingPageId = "1";
            var mockPageRepo = new Mock<IPageService>();
            var mockIssueRepo = new Mock<IIssueService>();
            mockPageRepo.Setup(repo => repo.GetWithId(nonExcistingPageId))
              .ReturnsAsync(null as Page);
            var controller = new PageController(mockPageRepo.Object, mockIssueRepo.Object);

            // Act
            var result = await controller.Delete(nonExcistingPageId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        #region private methods
        /// <summary>
        /// Gets a list of test pages for a give issue.
        /// </summary>
        /// <param name="issueId">The issue identifier.</param>
        /// <returns>A list of pages.</returns>
        private List<Page> GetTestPagesForIssue(string issueId)
        {
            var pages = new List<Page>();

            for (int i = 0; i < 10; i++)
            {
                pages.Add(new Page
                {
                    Id = $"id {i}",
                    IssueId = issueId,
                    PageNumber = i,
                    PageTitle = $"Test {i}",
                });
            }
            return pages;
        }


        /// <summary>
        /// Gets a test page.
        /// </summary>
        /// <param name="pageId">The comic identifier.</param>
        /// <param name="issueId">The issue identifier.</param>
        /// <returns>A page with the specified information.</returns>
        private Page GetTestPage(string pageId, string? issueId)
        {
            var page = new Page
            {
                Id = pageId,
                IssueId = issueId,
                PageNumber = 1,
                PageTitle = "Test",
            };

            return page;
        }
        #endregion
    }
}
