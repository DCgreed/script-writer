
using Microsoft.AspNetCore.Mvc;
using PocApi.Controllers;
using PocApi.Interfaces;
using PocApi.Models;

namespace PocApi.UnitTests.Controllers
{
    /// <summary>
    /// Test class for the actor controller.
    /// </summary>
    public class ActorControllerTest
    {
        [Fact]
        public async void GetSingleActor()
        {
            // Arrange
            var testId = "1";
            var mockActorRepo = new Mock<IActorService>();
            var mockComicRepo = new Mock<IComicService>();
            mockActorRepo.Setup(repo => repo.GetWithId(testId))
                .ReturnsAsync(GetTestActor(testId, testId));
            var controller = new ActorController(mockActorRepo.Object, mockComicRepo.Object);

            // Act
            var result = await controller.GetById(testId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Actor>>(result);
            var model = Assert.IsAssignableFrom<Actor>(actionResult.Value);
            Assert.Equal(testId, model.Id);
        }

        [Fact]
        public async void GetActorsForComic()
        {
            // Arrange
            var comicId = "1";
            var mockActorRepo = new Mock<IActorService>();
            var mockComicRepo = new Mock<IComicService>();
            mockComicRepo.Setup(repo => repo.GetWithId(comicId))
                .ReturnsAsync(GetTestComic(comicId));
            mockActorRepo.Setup(repo => repo.GetAllForComic(comicId))
                .ReturnsAsync(GetTestActorsForComic(comicId));
            var controller = new ActorController(mockActorRepo.Object, mockComicRepo.Object);

            // Act
            var result = await controller.GetAllForComic(comicId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<Actor>>>(result);
            var listResult = Assert.IsType<List<Actor>>(result.Value);
            Assert.True(listResult.All(x => x.ComicId == comicId));
        }

        [Fact]
        public async void CreateActorForComic()
        {
            // Arrange
            var excistingComicId = "1";
            var testId = "1";
            var newComicTest = GetTestActor(testId, null);
            var mockActorRepo = new Mock<IActorService>();
            var mockComicRepo = new Mock<IComicService>();
            mockActorRepo.Setup(repo => repo.Create(newComicTest))
                .Returns(Task.CompletedTask);
            mockComicRepo.Setup(repo => repo.GetWithId(excistingComicId))
                .ReturnsAsync(new Comic { Id = excistingComicId });
            var controller = new ActorController(mockActorRepo.Object, mockComicRepo.Object);

            // Act
            var result = await controller.Post(excistingComicId, newComicTest);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var model = Assert.IsType<Actor>(actionResult.Value);
            Assert.Equal(excistingComicId, model.ComicId);
        }

        [Fact]
        public async void CreateActorForNonExcistingComic_BadRequest()
        {
            // Arrange
            var testId = "1";
            var nonExcistingComicId = "1";
            var newActorTest = GetTestActor(testId, null);
            var mockActorRepo = new Mock<IActorService>();
            var mockComicRepo = new Mock<IComicService>();
            mockComicRepo.Setup(repo => repo.GetWithId(nonExcistingComicId))
                .ReturnsAsync(null as Comic);
            var controller = new ActorController(mockActorRepo.Object, mockComicRepo.Object);

            // Act
            var result = await controller.Post(nonExcistingComicId, newActorTest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void UpdatePanel()
        {
            // Arrange
            var excistingActorId = "1";
            var excistingActorTest = GetTestActor(excistingActorId, null);
            var mockActorRepo = new Mock<IActorService>();
            var mockComicRepo = new Mock<IComicService>();
            mockActorRepo.Setup(repo => repo.GetWithId(excistingActorId))
                .ReturnsAsync(GetTestActor(excistingActorId, null));
            mockActorRepo.Setup(repo => repo.Update(excistingActorId, excistingActorTest))
               .Returns(Task.CompletedTask);
            var controller = new ActorController(mockActorRepo.Object, mockComicRepo.Object);

            // Act
            var result = await controller.Update(excistingActorId, excistingActorTest);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void UpdatePage_NotFound()
        {
            // Arrange
            var nonExcistingActorId = "1";
            var nonExcistingActorTest = GetTestActor(nonExcistingActorId, null);
            var mockActorRepo = new Mock<IActorService>();
            var mockComicRepo = new Mock<IComicService>();
            mockActorRepo.Setup(repo => repo.GetWithId(nonExcistingActorId))
              .ReturnsAsync(null as Actor);
            var controller = new ActorController(mockActorRepo.Object, mockComicRepo.Object);

            // Act
            var result = await controller.Update(nonExcistingActorId, nonExcistingActorTest);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void DeletePage()
        {
            // Arrange
            var excistingActorId = "1";
            var mockActorRepo = new Mock<IActorService>();
            var mockComicRepo = new Mock<IComicService>();
            mockActorRepo.Setup(repo => repo.Delete(excistingActorId))
               .Returns(Task.CompletedTask);
            mockActorRepo.Setup(repo => repo.GetWithId(excistingActorId))
               .ReturnsAsync(GetTestActor(excistingActorId, null));
            var controller = new ActorController(mockActorRepo.Object, mockComicRepo.Object);

            // Act
            var result = await controller.Delete(excistingActorId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void DeleteIssue_NotFound()
        {
            // Arrange
            var nonExcistingActorId = "1";
            var mockActorRepo = new Mock<IActorService>();
            var mockComicRepo = new Mock<IComicService>();
            mockActorRepo.Setup(repo => repo.GetWithId(nonExcistingActorId))
              .ReturnsAsync(null as Actor);
            var controller = new ActorController(mockActorRepo.Object, mockComicRepo.Object);

            // Act
            var result = await controller.Delete(nonExcistingActorId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        #region private methods
        /// <summary>
        /// Gets a list of test actors for a comic.
        /// </summary>
        /// <param name="comicId">The comic identifier.</param>
        /// <returns>A list of actors.</returns>
        private List<Actor> GetTestActorsForComic(string comicId)
        {
            var actors = new List<Actor>();

            for (int i = 0; i < 10; i++)
            {
                actors.Add(new Actor
                {
                    Id = $"id {i}",
                    ComicId = comicId,
                });
            }
            return actors;
        }


        /// <summary>
        /// Gets a test actor.
        /// </summary>
        /// <param name="actorId">The actor identifier.</param>
        /// <param name="comicId">The comic identifier.</param>
        /// <returns>A actor with the specified information.</returns>
        private Actor GetTestActor(string actorId, string? comicId)
        {
            var actor = new Actor
            {
                Id = actorId,
                ComicId = comicId,
            };

            return actor;
        }

        /// <summary>
        /// Gets a test comic.
        /// </summary>
        /// <param name="comicId">The comic identifier.</param>
        /// <returns>A comic with the specified information.</returns>
        private Comic GetTestComic(string comicId)
        {
            var comic = new Comic
            {
                Id = comicId,
                createdBy = "Test",
                Title = "Test",
            };

            return comic;
        }
        #endregion
    }
}
