using Moq;
using Xunit;
using FlashQuizzz.API.Models;
using FlashQuizzz.API.DAO.Interfaces;
using FlashQuizzz.API.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlashQuizzz.API.Exceptions;

public class FlashCardServiceTests
{
    private readonly Mock<IFlashCardRepo> _mockRepo;
    private readonly FlashCardService _service;

    public FlashCardServiceTests()
    {
        _mockRepo = new Mock<IFlashCardRepo>();
        _service = new FlashCardService(null, _mockRepo.Object); // Pass null for DbContext since we are using a mock repo
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
            UserID = "user123",
            FlashCardCategoryID = 1 // Assuming this property is required
        };

        var newFlashCard = new FlashCard
        {
            FlashCardID = 1,
            FlashCardQuestion = flashCardDTO.FlashCardQuestion,
            FlashCardAnswer = flashCardDTO.FlashCardAnswer,
            CreatedDate = flashCardDTO.CreatedDate,
            UserID = flashCardDTO.UserID,
            FlashCardCategoryID = flashCardDTO.FlashCardCategoryID
        };

        _mockRepo.Setup(repo => repo.Create(It.IsAny<FlashCard>()))
            .ReturnsAsync(newFlashCard);

        // Act
        var result = await _service.CreateFlashCard(flashCardDTO);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newFlashCard.FlashCardQuestion, result.FlashCardQuestion);
        _mockRepo.Verify(repo => repo.Create(It.IsAny<FlashCard>()), Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldRemoveFlashCard()
    {
        // Arrange
        var flashCardID = 1;
        var flashCard = new FlashCard
        {
            FlashCardID = flashCardID,
            FlashCardQuestion = "What is the capital of France?",
            FlashCardAnswer = "Paris",
            CreatedDate = DateTime.UtcNow,
            UserID = "user123",
            FlashCardCategoryID = 1
        };

        _mockRepo.Setup(repo => repo.GetByID(flashCardID))
            .ReturnsAsync(flashCard);
        _mockRepo.Setup(repo => repo.Delete(flashCardID))
            .ReturnsAsync(flashCard);

        // Act
        var result = await _service.Delete(flashCardID);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(flashCardID, result.FlashCardID);
        _mockRepo.Verify(repo => repo.GetByID(flashCardID), Times.Once);
        _mockRepo.Verify(repo => repo.Delete(flashCardID), Times.Once);
    }

    [Fact]
    public async Task GetAllFlashCards_ShouldReturnAllFlashCards()
    {
        // Arrange
        var flashCards = new List<FlashCard>
        {
            new FlashCard
            {
                FlashCardID = 1,
                FlashCardQuestion = "Question 1",
                FlashCardAnswer = "Answer 1",
                CreatedDate = DateTime.UtcNow,
                UserID = "user123",
                FlashCardCategoryID = 1
            },
            new FlashCard
            {
                FlashCardID = 2,
                FlashCardQuestion = "Question 2",
                FlashCardAnswer = "Answer 2",
                CreatedDate = DateTime.UtcNow,
                UserID = "user123",
                FlashCardCategoryID = 1
            }
        };

        _mockRepo.Setup(repo => repo.GetAll())
            .ReturnsAsync(flashCards);

        // Act
        var result = await _service.GetAllFlashCards();

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count);
        _mockRepo.Verify(repo => repo.GetAll(), Times.Once);
    }

    //[Fact]
public async Task Update_ShouldModifyFlashCard()
{
    // Arrange
    var flashCardID = 1;
    var existingFlashCard = new FlashCard
    {
        FlashCardID = flashCardID,
        FlashCardQuestion = "Old Question",
        FlashCardAnswer = "Old Answer",
        CreatedDate = DateTime.UtcNow,
        UserID = "user123",
        FlashCardCategoryID = 1
    };

    var updatedFlashCardDTO = new FlashCardDTO
    {
        FlashCardQuestion = "Updated Question",
        FlashCardAnswer = "Updated Answer",
        CreatedDate = DateTime.UtcNow,
        UserID = "user123",
        FlashCardCategoryID = 1
    };

    // Mock repository methods
    _mockRepo.Setup(repo => repo.GetByID(flashCardID))
        .ReturnsAsync(existingFlashCard);
    _mockRepo.Setup(repo => repo.Update(flashCardID, It.Is<FlashCard>(fc =>
        fc.FlashCardID == flashCardID &&
        fc.FlashCardQuestion == updatedFlashCardDTO.FlashCardQuestion &&
        fc.FlashCardAnswer == updatedFlashCardDTO.FlashCardAnswer &&
        fc.CreatedDate == updatedFlashCardDTO.CreatedDate &&
        fc.UserID == updatedFlashCardDTO.UserID &&
        fc.FlashCardCategoryID == updatedFlashCardDTO.FlashCardCategoryID)))
        .ReturnsAsync(true);

    // Act
    var result = await _service.Update(flashCardID, updatedFlashCardDTO);

    // Assert
    Assert.True(result);
    _mockRepo.Verify(repo => repo.Update(flashCardID, It.IsAny<FlashCard>()), Times.Once);
}

}
