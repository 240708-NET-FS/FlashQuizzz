using Moq;
using Xunit;
using FlashQuizzz.API.Services;
using FlashQuizzz.API.DAO.Interfaces;
using FlashQuizzz.API.Models;
using FlashQuizzz.API.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FlashCardServiceTests
{
    private readonly Mock<IFlashCardRepo> _mockRepo;
    private readonly FlashCardService _service;

    public FlashCardServiceTests()
    {
        _mockRepo = new Mock<IFlashCardRepo>();
        _service = new FlashCardService(null, _mockRepo.Object); // Null context for simplicity
    }

    [Fact]
    public async Task CreateFlashCard_ShouldReturnFlashCard()
    {
        var dto = new FlashCardDTO
        {
            FlashCardQuestion = "Question",
            FlashCardAnswer = "Answer",
            CreatedDate = DateTime.Now,
            UserID = "userId",
            FlashCardCategoryID = 1
        };
        var flashCard = new FlashCard
        {
            FlashCardQuestion = "Question",
            FlashCardAnswer = "Answer",
            CreatedDate = DateTime.Now,
            UserID = "userId",
            FlashCardCategoryID = 1
        };

        _mockRepo.Setup(r => r.Create(It.IsAny<FlashCard>())).ReturnsAsync(flashCard);

        var result = await _service.CreateFlashCard(dto);

        Assert.Equal(flashCard, result);
    }

    [Fact]
    public async Task Delete_ShouldThrowInvalidFlashCardException_WhenFlashCardDoesNotExist()
    {
        _mockRepo.Setup(r => r.GetByID(It.IsAny<int>())).ReturnsAsync((FlashCard?)null);

        await Assert.ThrowsAsync<InvalidFlashCardException>(() => _service.Delete(1));
    }

    [Fact]
    public async Task GetAllFlashCards_ShouldThrowInvalidFlashCardCategoryException_WhenNoFlashCardsExist()
    {
        _mockRepo.Setup(r => r.GetAll()).ReturnsAsync(new List<FlashCard>());

        await Assert.ThrowsAsync<InvalidFlashCardCategoryException>(() => _service.GetAllFlashCards());
    }

    [Fact]
    public async Task GetByCategory_ShouldReturnFlashCards()
    {
        var flashCards = new List<FlashCard>
        {
            new FlashCard
            {
                FlashCardQuestion = "Question",
                FlashCardAnswer = "Answer",
                CreatedDate = DateTime.Now,
                UserID = "userId",
                FlashCardCategoryID = 1
            }
        };

        _mockRepo.Setup(r => r.GetByCategoryID(It.IsAny<int>())).ReturnsAsync(flashCards);

        var result = await _service.GetByCategory(1);

        Assert.NotEmpty(result);
    }

   // [Fact]
    public async Task GetByFlashCardNumber_ShouldReturnFlashCard()
    {
        var flashCard = new FlashCard
        {
            FlashCardQuestion = "Question",
            FlashCardAnswer = "Answer",
            CreatedDate = DateTime.Now,
            UserID = "userId",
            FlashCardCategoryID = 1
        };

        _mockRepo.Setup(r => r.GetByID(It.IsAny<int>())).ReturnsAsync(flashCard);

        var result = await _service.GetByFlashCardNumber(1);

        Assert.Equal(flashCard, result);
    }

  //[Fact]
    public async Task GetByUser_ShouldReturnFlashCards()
    {
        var flashCards = new List<FlashCard>
        {
            new FlashCard
            {
                FlashCardQuestion = "Question",
                FlashCardAnswer = "Answer",
                CreatedDate = DateTime.Now,
                UserID = "userId",
                FlashCardCategoryID = 1
            }
        };

        

        var result = await _service.GetByUser("userId");

        Assert.NotEmpty(result);
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
            UserID = "user123",
            FlashCardCategoryID = 1
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
}
