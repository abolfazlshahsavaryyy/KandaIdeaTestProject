using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using AgriculturalLandManagement.Controllers;
using AgriculturalLandManagement.Repositories;
using AgriculturalLandManagement.Models;
using AgriculturalLandManagement.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgriculturalLandManagement.Tests
{
    public class LandControllerTests
    {
        [Fact]
        public async Task GetAll_ReturnsAllLands()
        {
            // Arrange
            var mockRepo = new Mock<ILandRepository>();
            mockRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Land>
                {
                    new Land { Id = 1, OwnerName = "John", Production = "Wheat", X1=0,Y1=0,X2=10,Y2=10,Area=100 },
                    new Land { Id = 2, OwnerName = "Jane", Production = "Corn", X1=0,Y1=0,X2=20,Y2=20,Area=400 }
                });

            var controller = new LandController(mockRepo.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var lands = Assert.IsAssignableFrom<IEnumerable<Land>>(okResult.Value);
            Assert.Equal(2, ((List<Land>)lands).Count);
        }

        [Fact]
        public async Task Create_ValidLand_ReturnsCreatedLand()
        {
            // Arrange
            var dto = new CreateLandDto
            {
                OwnerName = "Alice",
                Production = "Rice",
                X1 = 0,
                Y1 = 0,
                X2 = 5,
                Y2 = 5
            };

            var land = new Land
            {
                Id = 1,
                OwnerName = dto.OwnerName,
                Production = dto.Production,
                X1 = dto.X1,
                Y1 = dto.Y1,
                X2 = dto.X2,
                Y2 = dto.Y2,
                Area = 25 // computed
            };

            var mockRepo = new Mock<ILandRepository>();
            mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Land>())).ReturnsAsync(land);

            var controller = new LandController(mockRepo.Object);

            // Act
            var result = await controller.Create(dto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedLand = Assert.IsType<Land>(createdAtActionResult.Value);
            Assert.Equal(25, returnedLand.Area);
            Assert.Equal("Alice", returnedLand.OwnerName);
        }
    }
}
