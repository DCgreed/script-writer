using PocApi.Controllers;
using PocApi.Models;
using PocApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PocApi.UnitTests.Controllers
{
    public class ComicControllerTest
    {
        [Fact]
        public async void MyFirstTest()
        {
            // Arrange
            var testId = "1";
            var mockRepo = new Mock<IComicService>();
            mockRepo.Setup(repo => repo.GetWithId(testId))
                .ReturnsAsync(GetTestComic(testId));
            var controller = new ComicsController(mockRepo.Object);

            // Act
            var result = await controller.GetById(testId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Comic>>(result);
            var model = Assert.IsAssignableFrom<Comic>(actionResult.Value);
            Assert.Equal(testId, model.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comicId"></param>
        /// <returns></returns>
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
    }

    
}