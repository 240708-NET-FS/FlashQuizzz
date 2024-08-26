using Moq;
using Xunit;
using FlashQuizzz.API.Models;
using FlashQuizzz.API.Services;
using FlashQuizzz.API.DAO;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using FlashQuizzz.API.DAO.Interfaces;
using FlashQuizzz.API.Exceptions;

namespace FlashQuizzz.Tests
{
    public class FlashCardServiceTests
    {
        private readonly FlashCardService _service;
        private readonly DbContextOptions<AppDbContext> _contextOptions;

        public FlashCardServiceTests()
        {
            _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(_contextOptions))
            {
                context.Database.EnsureCreated();
            }

            _service = new FlashCardService(new AppDbContext(_contextOptions));
        }

        [Fact]
        public async Task CreateFlashCard_ShouldAddFlashCard()
        {
            // Arrange
            var flashCardDTO = new FlashCardDTO
            {
                FlashCardQuestion = "What is the capital of France?",
                FlashCardAnswer = "Paris",
                CreatedDate = DateTime.UtcNow,
                UserID = "user123"
            };

            // Act
            var flashCard = await _service.CreateFlashCard(flashCardDTO);

            // Assert
            using (var context = new AppDbContext(_contextOptions))
            {
                var addedFlashCard = await context.FlashCard.FindAsync(flashCard.FlashCardID);
                Assert.NotNull(addedFlashCard);
                Assert.Equal(flashCardDTO.FlashCardQuestion, addedFlashCard.FlashCardQuestion);
            }
        }

        [Fact]
        public async Task Delete_ShouldRemoveFlashCard()
        {
            // Arrange
            var flashCardDTO = new FlashCardDTO
            {
                FlashCardQuestion = "What is the capital of France?",
                FlashCardAnswer = "Paris",
                CreatedDate = DateTime.UtcNow,
                UserID = "user123"
            };
            var flashCard = await _service.CreateFlashCard(flashCardDTO);

            // Act
            var deletedFlashCard = await _service.Delete(flashCard.FlashCardID);

            // Assert
            Assert.NotNull(deletedFlashCard);
            Assert.Equal(flashCard.FlashCardID, deletedFlashCard.FlashCardID);

            using (var context = new AppDbContext(_contextOptions))
            {
                var removedFlashCard = await context.FlashCard.FindAsync(flashCard.FlashCardID);
                Assert.Null(removedFlashCard);
            }
        }

       //[Fact]
        public async Task GetAllFlashCards_ShouldReturnAllFlashCards()
        {
            // Arrange
            await _service.CreateFlashCard(new FlashCardDTO
            {
                FlashCardQuestion = "Question 1",
                FlashCardAnswer = "Answer 1",
                CreatedDate = DateTime.UtcNow,
                UserID = "user123"
            });
            await _service.CreateFlashCard(new FlashCardDTO
            {
                FlashCardQuestion = "Question 2",
                FlashCardAnswer = "Answer 2",
                CreatedDate = DateTime.UtcNow,
                UserID = "user123"
            });

            // Act
            var flashCards = await _service.GetAllFlashCards();

            // Assert
            Assert.NotEmpty(flashCards);
            Assert.Equal(2, flashCards.Count);
        }

        [Fact]
        public async Task Update_ShouldModifyFlashCard()
        {
            // Arrange
            var flashCardDTO = new FlashCardDTO
            {
                FlashCardQuestion = "What is the capital of France?",
                FlashCardAnswer = "Paris",
                CreatedDate = DateTime.UtcNow,
                UserID = "user123"
            };
            var flashCard = await _service.CreateFlashCard(flashCardDTO);

            var updatedDTO = new FlashCardDTO
            {
                FlashCardQuestion = "What is the largest planet?",
                FlashCardAnswer = "Jupiter",
                CreatedDate = DateTime.UtcNow,
                UserID = "user123"
            };

            // Act
            var result = await _service.Update(flashCard.FlashCardID, updatedDTO);

            // Assert
            Assert.True(result);
            using (var context = new AppDbContext(_contextOptions))
            {
                var updatedFlashCard = await context.FlashCard.FindAsync(flashCard.FlashCardID);
                Assert.Equal(updatedDTO.FlashCardQuestion, updatedFlashCard.FlashCardQuestion);
                Assert.Equal(updatedDTO.FlashCardAnswer, updatedFlashCard.FlashCardAnswer);
            }
        }
    }
}
