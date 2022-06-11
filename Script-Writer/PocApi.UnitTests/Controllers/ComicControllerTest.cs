using PocApi.Controllers;
using PocApi.Models;
using PocApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PocApi.UnitTests.Controllers
{
    public class ComicControllerTest
    {
        [Fact]
        public async void GetSingleComic()
        {
            // Arrange
            var testId = "1";
            var mockRepo = new Mock<IComicService>();
            mockRepo.Setup(repo => repo.GetWithId(testId))
                .ReturnsAsync(GetTestComic(testId));
            var controller = new ComicController(mockRepo.Object);

            // Act
            var result = await controller.GetById(testId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Comic>>(result);
            var model = Assert.IsAssignableFrom<Comic>(actionResult.Value);
            Assert.Equal(testId, model.Id);
        }

        [Fact]
        public async void GetAllComics()
        {
            // Arrange
            var mockComicRepo = new Mock<IComicService>();
            mockComicRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(GetTestComics());
            var controller = new ComicController(mockComicRepo.Object);

            // Act
            var result = await controller.Get();

            // Assert
            Assert.IsType<List<Comic>>(result);
        }

        [Fact]
        public async void CreateComic()
        {
            // Arrange
            var newComicTest = GetTestComic(null);
            var mockComicRepo = new Mock<IComicService>();
            mockComicRepo.Setup(repo => repo.Create(newComicTest))
                .Returns(Task.CompletedTask);
            var controller = new ComicController(mockComicRepo.Object);

            // Act
            var result = await controller.Post(newComicTest);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.IsType<Comic>(actionResult.Value);
        }

        [Fact]
        public async void UpdateComic()
        {
            // Arrange
            var excistingComicId = "1";
            var excistingComicTest = GetTestComic(excistingComicId);
            var mockComicRepo = new Mock<IComicService>();
            mockComicRepo.Setup(repo => repo.GetWithId(excistingComicId))
                .ReturnsAsync(GetTestComic(excistingComicId));
            mockComicRepo.Setup(repo => repo.Update(excistingComicId, excistingComicTest))
               .Returns(Task.CompletedTask);
            var controller = new ComicController(mockComicRepo.Object);

            // Act
            var result = await controller.Update(excistingComicId, excistingComicTest);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void UpdateComic_NotFound()
        {
            // Arrange
            var nonExcistingComicId = "1";
            var nonExcistingComicTest = GetTestComic(nonExcistingComicId);
            var mockComicRepo = new Mock<IComicService>();
            mockComicRepo.Setup(repo => repo.GetWithId(nonExcistingComicId))
              .ReturnsAsync(null as Comic);
            var controller = new ComicController(mockComicRepo.Object);

            // Act
            var result = await controller.Update(nonExcistingComicId, nonExcistingComicTest);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void DeletePage()
        {
            // Arrange
            var excistingComicId = "1";
            var mockComicRepo = new Mock<IComicService>();
            mockComicRepo.Setup(repo => repo.GetWithId(excistingComicId))
               .ReturnsAsync(GetTestComic(excistingComicId));
            mockComicRepo.Setup(repo => repo.Delete(excistingComicId))
               .Returns(Task.CompletedTask);
            var controller = new ComicController(mockComicRepo.Object);

            // Act
            var result = await controller.Delete(excistingComicId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void DeleteIssue_NotFound()
        {
            // Arrange
            var nonExcistingComicId = "1";
            var mockComicRepo = new Mock<IComicService>();
            mockComicRepo.Setup(repo => repo.GetWithId(nonExcistingComicId))
              .ReturnsAsync(null as Comic);
            var controller = new ComicController(mockComicRepo.Object);

            // Act
            var result = await controller.Delete(nonExcistingComicId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        #region private methods
        /// <summary>
        /// Gets a list of test comics.
        /// </summary>
        /// <returns>A list of comics.</returns>
        private List<Comic> GetTestComics()
        {
            var comics = new List<Comic>();

            for (int i = 0; i < 10; i++)
            {
                comics.Add(new Comic
                {
                    Id = $"id {i}",
                    createdBy = "Test",
                    Title = "Test"
                });
            }
            return comics;
        }

        /// <summary>
        /// Gets a test comic.
        /// </summary>
        /// <param name="comicId">The comic identifier.</param>
        /// <returns>A comic with the specified information.</returns>
        private Comic GetTestComic(string? comicId)
        {
            var comic = new Comic
            {
                Id = comicId,
                createdBy = "Test",
                Title = "Test"
            };

            return comic;
        }
        #endregion
    }
}