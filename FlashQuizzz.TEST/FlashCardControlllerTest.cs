using Moq;
using Xunit;
using FlashQuizzz.API.Controllers;
using FlashQuizzz.API.Models;
using FlashQuizzz.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace FlashQuizzz.API.Tests
{
    public class FlashCardControllerTests
    {
        private readonly FlashCardController _controller;
        private readonly Mock<IFlashCardService> _mockService;

        public FlashCardControllerTests()
        {
            _mockService = new Mock<IFlashCardService>();
            _controller = new FlashCardController(_mockService.Object);
        }

        [Fact]
        public async Task CreateFlashCard_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var newFlashCard = new FlashCardDTO 
            { 
                FlashCardQuestion = "Question", 
                FlashCardAnswer = "Answer", 
                CreatedDate = DateTime.UtcNow, 
                UserID = "user1" 
            };
            var createdFlashCard = new FlashCard 
            { 
                FlashCardID = 1, 
                FlashCardQuestion = "Question", 
                FlashCardAnswer = "Answer", 
                CreatedDate = DateTime.UtcNow, 
                UserID = "user1" 
            };
            _mockService.Setup(service => service.CreateFlashCard(It.IsAny<FlashCardDTO>()))
                        .ReturnsAsync(createdFlashCard);

            // Act
            var result = await _controller.CreateFlashCard(newFlashCard);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(FlashCardController.GetFlashCardById), actionResult.ActionName);
            Assert.Equal(createdFlashCard, actionResult.Value);
        }

        [Fact]
        public async Task CreateFlashCard_ReturnsBadRequest_WhenDataIsNull()
        {
            // Act
            var result = await _controller.CreateFlashCard(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteFlashCard_ReturnsNoContent()
        {
            // Arrange
            var flashCardId = 1;
            var flashCard = new FlashCard 
            { 
                FlashCardID = flashCardId, 
                FlashCardQuestion = "Question", 
                FlashCardAnswer = "Answer", 
                CreatedDate = DateTime.UtcNow, 
                UserID = "user1" 
            };
            _mockService.Setup(service => service.Delete(flashCardId))
                        .ReturnsAsync(flashCard);

            // Act
            var result = await _controller.DeleteFlashCard(flashCardId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteFlashCard_ReturnsNotFound_WhenFlashCardDoesNotExist()
        {
            // Arrange
            var flashCardId = 1;
            _mockService.Setup(service => service.Delete(flashCardId))
                        .ReturnsAsync((FlashCard)null);

            // Act
            var result = await _controller.DeleteFlashCard(flashCardId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAllFlashCards_ReturnsOkResultWithFlashCards()
        {
            // Arrange
            var flashCards = new List<FlashCard>
            {
                new FlashCard 
                { 
                    FlashCardID = 1, 
                    FlashCardQuestion = "Question1", 
                    FlashCardAnswer = "Answer1", 
                    CreatedDate = DateTime.UtcNow, 
                    UserID = "user1" 
                }
            };
            _mockService.Setup(service => service.GetAllFlashCards())
                        .ReturnsAsync(flashCards);

            // Act
            var result = await _controller.GetAllFlashCards();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(flashCards, actionResult.Value);
        }

        [Fact]
        public async Task GetFlashCardById_ReturnsOkResultWithFlashCard()
        {
            // Arrange
            var flashCardId = 1;
            var flashCard = new FlashCard 
            { 
                FlashCardID = flashCardId, 
                FlashCardQuestion = "Question", 
                FlashCardAnswer = "Answer", 
                CreatedDate = DateTime.UtcNow, 
                UserID = "user1" 
            };
            _mockService.Setup(service => service.GetFlashCardById(flashCardId))
                        .ReturnsAsync(flashCard);

            // Act
            var result = await _controller.GetFlashCardById(flashCardId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(flashCard, actionResult.Value);
        }

        [Fact]
        public async Task GetFlashCardById_ReturnsNotFound_WhenFlashCardDoesNotExist()
        {
            // Arrange
            var flashCardId = 1;
            _mockService.Setup(service => service.GetFlashCardById(flashCardId))
                        .ReturnsAsync((FlashCard)null);

            // Act
            var result = await _controller.GetFlashCardById(flashCardId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateFlashCard_ReturnsNoContent()
        {
            // Arrange
            var flashCardId = 1;
            var updatedFlashCard = new FlashCardDTO 
            { 
                FlashCardQuestion = "Updated Question", 
                FlashCardAnswer = "Updated Answer", 
                CreatedDate = DateTime.UtcNow, 
                UserID = "user1" 
            };
            _mockService.Setup(service => service.Update(flashCardId, updatedFlashCard))
                        .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateFlashCard(flashCardId, updatedFlashCard);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateFlashCard_ReturnsBadRequest_WhenDataIsNull()
        {
            // Act
            var result = await _controller.UpdateFlashCard(1, null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetByUser_ReturnsOkResultWithFlashCards()
        {
            // Arrange
            var userId = "user1";
            var flashCards = new List<FlashCard>
            {
                new FlashCard 
                { 
                    FlashCardID = 1, 
                    FlashCardQuestion = "Question1", 
                    FlashCardAnswer = "Answer1", 
                    CreatedDate = DateTime.UtcNow, 
                    UserID = userId 
                }
            };
            _mockService.Setup(service => service.GetByUser(userId))
                        .ReturnsAsync(flashCards);

            // Act
            var result = await _controller.GetByUser(userId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(flashCards, actionResult.Value);
        }
    }
}
