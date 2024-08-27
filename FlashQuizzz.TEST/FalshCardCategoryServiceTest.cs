using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using FlashQuizzz.API.DAO.Interfaces;
using FlashQuizzz.API.Models;
using FlashQuizzz.API.Services;
using FlashQuizzz.API.Utilities;
using FlashQuizzz.API.Exceptions;

namespace FlashQuizzz.API.Tests
{
    public class FlashCardCategoryServiceTests
    {
        private readonly Mock<IFlashCardCategoryRepo> _mockCategoryRepo;
        private readonly FlashCardCategoryService _service;

        public FlashCardCategoryServiceTests()
        {
            _mockCategoryRepo = new Mock<IFlashCardCategoryRepo>();
            _service = new FlashCardCategoryService(_mockCategoryRepo.Object);
        }

        [Fact]
        public async Task CreateFlashCardCategory_ValidDTO_CreatesCategory()
        {
            // Arrange
            var flashCardCategoryDTO = new FlashCardCategoryDTO
            {
                FlashCardCategoryID = 1,
                FlashCardCategoryName = "Science",
                CreatedDate = DateTime.UtcNow,
                FlashCardCategoryStatus = true // Corrected to bool
            };

            _mockCategoryRepo.Setup(repo => repo.GetByID(1)).ReturnsAsync((FlashCardCategory?)null);
            _mockCategoryRepo.Setup(repo => repo.GetByName("Science")).ReturnsAsync((FlashCardCategory?)null);
            _mockCategoryRepo.Setup(repo => repo.Create(It.IsAny<FlashCardCategory>())).ReturnsAsync(new FlashCardCategory
            {
                FlashCardCategoryID = 1,
                FlashCardCategoryName = "Science",
                CreatedDate = DateTime.UtcNow,
                FlashCardCategoryStatus = true // Corrected to bool
            });

            // Act
            var result = await _service.CreateFlashCardCategory(flashCardCategoryDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Science", result.FlashCardCategoryName);
        }

       [Fact]
        public async Task CreateFlashCardCategory_DuplicateID_ThrowsException()
        {
            // Arrange
            var flashCardCategoryDTO = new FlashCardCategoryDTO
            {
                FlashCardCategoryID = 1,
                FlashCardCategoryName = "Science",
                CreatedDate = DateTime.UtcNow,
                FlashCardCategoryStatus = true // Corrected to bool
            };

            _mockCategoryRepo.Setup(repo => repo.GetByID(1)).ReturnsAsync(new FlashCardCategory
            {
                FlashCardCategoryID = 1,
                FlashCardCategoryName = "Science",
                CreatedDate = DateTime.UtcNow,
                FlashCardCategoryStatus = true // Corrected to bool
            });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidFlashCardCategoryException>(() => _service.CreateFlashCardCategory(flashCardCategoryDTO));
        }

        [Fact]
        public async Task Delete_ExistingCategory_ReturnsCategory()
        {
            // Arrange
            var flashCardCategory = new FlashCardCategory
            {
                FlashCardCategoryID = 1,
                FlashCardCategoryName = "Science",
                CreatedDate = DateTime.UtcNow,
                FlashCardCategoryStatus = true // Corrected to bool
            };

            _mockCategoryRepo.Setup(repo => repo.GetByID(1)).ReturnsAsync(flashCardCategory);
            _mockCategoryRepo.Setup(repo => repo.Delete(1)).ReturnsAsync(flashCardCategory);

            // Act
            var result = await _service.Delete(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.FlashCardCategoryID);
        }

        [Fact]
        public async Task Delete_NonExistingCategory_ThrowsException()
        {
            // Arrange
            _mockCategoryRepo.Setup(repo => repo.GetByID(1)).ReturnsAsync((FlashCardCategory?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidFlashCardCategoryException>(() => _service.Delete(1));
        }

        [Fact]
        public async Task GetAllCategories_ReturnsCategories()
        {
            // Arrange
            var categories = new List<FlashCardCategory>
            {
                new FlashCardCategory { FlashCardCategoryID = 1, FlashCardCategoryName = "Science", CreatedDate = DateTime.UtcNow, FlashCardCategoryStatus = true }, // Corrected to bool
                new FlashCardCategory { FlashCardCategoryID = 2, FlashCardCategoryName = "Math", CreatedDate = DateTime.UtcNow, FlashCardCategoryStatus = true } // Corrected to bool
            };

            _mockCategoryRepo.Setup(repo => repo.GetAll()).ReturnsAsync(categories);

            // Act
            var result = await _service.GetAllCategories();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllCategories_NoCategories_ThrowsException()
        {
            // Arrange
            _mockCategoryRepo.Setup(repo => repo.GetAll()).ReturnsAsync(new List<FlashCardCategory>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidFlashCardCategoryException>(() => _service.GetAllCategories());
        }

        [Fact]
        public async Task GetByFlashCardCategoryByName_ExistingName_ReturnsCategory()
        {
            // Arrange
            var flashCardCategory = new FlashCardCategory
            {
                FlashCardCategoryID = 1,
                FlashCardCategoryName = "Science",
                CreatedDate = DateTime.UtcNow,
                FlashCardCategoryStatus = true // Corrected to bool
            };

            _mockCategoryRepo.Setup(repo => repo.GetByName("Science")).ReturnsAsync(flashCardCategory);

            // Act
            var result = await _service.GetByFlashCardCategoryByName("Science");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Science", result.FlashCardCategoryName);
        }

       [Fact]
        public async Task GetByFlashCardCategoryByName_NonExistingName_ThrowsException()
        {
            // Arrange
            _mockCategoryRepo.Setup(repo => repo.GetByName("NonExistent")).ReturnsAsync((FlashCardCategory?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidFlashCardCategoryException>(() => _service.GetByFlashCardCategoryByName("NonExistent"));
        }

       [Fact]
        public async Task GetFlashCardCategoryById_ExistingId_ReturnsCategory()
        {
            // Arrange
            var flashCardCategory = new FlashCardCategory
            {
                FlashCardCategoryID = 1,
                FlashCardCategoryName = "Science",
                CreatedDate = DateTime.UtcNow,
                FlashCardCategoryStatus = true // Corrected to bool
            };

            _mockCategoryRepo.Setup(repo => repo.GetByID(1)).ReturnsAsync(flashCardCategory);

            // Act
            var result = await _service.GetFlashCardCategoryById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Science", result.FlashCardCategoryName);
        }

        [Fact]
        public async Task GetFlashCardCategoryById_NonExistingId_ThrowsException()
        {
            // Arrange
            _mockCategoryRepo.Setup(repo => repo.GetByID(1)).ReturnsAsync((FlashCardCategory?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidFlashCardCategoryException>(() => _service.GetFlashCardCategoryById(1));
        }

        [Fact]
        public async Task Update_ValidDTO_UpdatesCategory()
        {
            // Arrange
            var flashCardCategoryDTO = new FlashCardCategoryDTO
            {
                FlashCardCategoryName = "Updated Science",
                CreatedDate = DateTime.UtcNow,
                FlashCardCategoryStatus = true // Corrected to bool
            };

            _mockCategoryRepo.Setup(repo => repo.Update(1, It.IsAny<FlashCardCategory>())).ReturnsAsync(true);

            // Act
            var result = await _service.Update(1, flashCardCategoryDTO);

            // Assert
            Assert.True(result);
        }
    }
}
