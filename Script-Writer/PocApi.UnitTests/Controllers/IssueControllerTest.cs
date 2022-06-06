
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
                .ReturnsAsync(GetTestIssue(testId));
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
                    CreatedDate = DateTime.Now,
                });
            }
            return issues;
        }


        /// <summary>
        /// Gets a test issue.
        /// </summary>
        /// <param name="issueId">The issue identifier.</param>
        /// <returns>A issue with the specified information.</returns>
        private Issue GetTestIssue(string issueId)
        {
            var issue = new Issue
            {
                Id = issueId,
                ComicId = "test1",
                IssueNumber = 1,
                Title = "Test",
                CreatedDate = DateTime.Now,
            };

            return issue;
        }
    }
}
