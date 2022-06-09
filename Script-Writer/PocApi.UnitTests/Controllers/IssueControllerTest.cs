
using Microsoft.AspNetCore.Mvc;
using PocApi.Controllers;
using PocApi.Interfaces;
using PocApi.Models;

namespace PocApi.UnitTests.Controllers
{
    /// <summary>
    /// Test class for the issue controller.
    /// </summary>
    public class IssueControllerTest
    {
        [Fact]
        public async void GetSingleIssue()
        {
            // Arrange
            var testId = "1";
            var mockIssueRepo = new Mock<IIssueService>();
            var mockComicRepo = new Mock<IComicService>();
            mockIssueRepo.Setup(repo => repo.GetWithId(testId))
                .ReturnsAsync(GetTestIssue(testId, testId));
            var controller = new IssueController(mockIssueRepo.Object, mockComicRepo.Object);

            // Act
            var result = await controller.GetById(testId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Issue>>(result);
            var model = Assert.IsAssignableFrom<Issue>(actionResult.Value);
            Assert.Equal(testId, model.Id);
        }

        [Fact]
        public async void GetIssuesForComic()
        {
            // Arrange
            var testId = "1";
            var mockIssueRepo = new Mock<IIssueService>();
            var mockComicRepo = new Mock<IComicService>();
            mockIssueRepo.Setup(repo => repo.GetAllForComic(testId))
                .ReturnsAsync(GetTestIssuesForComic(testId));
            var controller = new IssueController(mockIssueRepo.Object, mockComicRepo.Object);

            // Act
            var result = await controller.GetByComicId(testId);

            // Assert
            var listResult = Assert.IsType<List<Issue>>(result);
            Assert.True(listResult.All(x => x.ComicId == testId));
        }

        [Fact]
        public async void CreateIssueForComic()
        {
            // Arrange
            var comicId = "1";
            var newIssueTest = GetTestIssue("issue 1", null);
            var mockIssueRepo = new Mock<IIssueService>();
            var mockComicRepo = new Mock<IComicService>();
            mockIssueRepo.Setup(repo => repo.Create(newIssueTest))
                .Returns(Task.CompletedTask);
            mockComicRepo.Setup(repo => repo.GetWithId(comicId))
                .ReturnsAsync(new Comic { Id = comicId });
            var controller = new IssueController(mockIssueRepo.Object, mockComicRepo.Object);

            // Act
            var result = await controller.Post(comicId, newIssueTest);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var model = Assert.IsType<Issue>(actionResult.Value);
            Assert.Equal(comicId, model.ComicId);
        }

        [Fact]
        public async void CreateIssueForNonExcistingComic_BadRequest()
        {
            // Arrange
            var nonExcistingComicId = "1";
            var newIssueTest = GetTestIssue("issue 1", null);
            var mockIssueRepo = new Mock<IIssueService>();
            var mockComicRepo = new Mock<IComicService>();
            var controller = new IssueController(mockIssueRepo.Object, mockComicRepo.Object);

            // Act
            var result = await controller.Post(nonExcistingComicId, newIssueTest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void UpdateIssue()
        {
            // Arrange
            var excistingIssueId = "1";
            var excistingIssueTest = GetTestIssue(excistingIssueId, null);
            var mockIssueRepo = new Mock<IIssueService>();
            var mockComicRepo = new Mock<IComicService>();
            mockIssueRepo.Setup(repo => repo.Update(excistingIssueId, excistingIssueTest))
               .Returns(Task.CompletedTask);
            mockIssueRepo.Setup(repo => repo.GetWithId(excistingIssueId))
                .ReturnsAsync(GetTestIssue(excistingIssueId, null));
            var controller = new IssueController(mockIssueRepo.Object, mockComicRepo.Object);

            // Act
            var result = await controller.Update(excistingIssueId, excistingIssueTest);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void UpdateIssue_NotFound()
        {
            // Arrange
            var nonExcistingIssueId = "1";
            var nonExcistingIssueTest = GetTestIssue(nonExcistingIssueId, null);
            var mockIssueRepo = new Mock<IIssueService>();
            var mockComicRepo = new Mock<IComicService>();
            mockIssueRepo.Setup(repo => repo.GetWithId(nonExcistingIssueId))
              .ReturnsAsync(null as Issue);
            var controller = new IssueController(mockIssueRepo.Object, mockComicRepo.Object);

            // Act
            var result = await controller.Update(nonExcistingIssueId, nonExcistingIssueTest);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void DeleteIssue()
        {
            // Arrange
            var excistingIssueId = "1";
            var mockIssueRepo = new Mock<IIssueService>();
            var mockComicRepo = new Mock<IComicService>();
            mockIssueRepo.Setup(repo => repo.Delete(excistingIssueId))
               .Returns(Task.CompletedTask);
            mockIssueRepo.Setup(repo => repo.GetWithId(excistingIssueId))
               .ReturnsAsync(GetTestIssue(excistingIssueId, null));
            var controller = new IssueController(mockIssueRepo.Object, mockComicRepo.Object);

            // Act
            var result = await controller.Delete(excistingIssueId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void DeleteIssue_NotFound()
        {
            // Arrange
            var nonExcistingIssueId = "1";
            var mockIssueRepo = new Mock<IIssueService>();
            var mockComicRepo = new Mock<IComicService>();
            mockIssueRepo.Setup(repo => repo.GetWithId(nonExcistingIssueId))
              .ReturnsAsync(null as Issue);
            var controller = new IssueController(mockIssueRepo.Object, mockComicRepo.Object);

            // Act
            var result = await controller.Delete(nonExcistingIssueId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        #region private methods
        /// <summary>
        /// Gets a list of test issues for a give comic.
        /// </summary>
        /// <param name="comicId">The comic identifier.</param>
        /// <returns>A list of issues.</returns>
        private List<Issue> GetTestIssuesForComic(string comicId)
        {
            var issues = new List<Issue>();

            for (int i = 0; i < 10; i++)
            {
                issues.Add(new Issue
                {
                    Id = $"id {i}",
                    ComicId = comicId,
                    IssueNumber = i,
                    Title = $"Test {i}",
                });
            }
            return issues;
        }


        /// <summary>
        /// Gets a test issue.
        /// </summary>
        /// <param name="issueId">The issue identifier.</param>
        /// <param name="comicId">The comic identifier.</param>
        /// <returns>A issue with the specified information.</returns>
        private Issue GetTestIssue(string issueId, string? comicId)
        {
            var issue = new Issue
            {
                Id = issueId,
                ComicId = comicId,
                IssueNumber = 1,
                Title = "Test",
            };

            return issue;
        }
        #endregion
    }
}
