using AutoMapper;
using Haiku.API.Controllers;
using Haiku.API.Dtos;
using Haiku.API.Models;
using Haiku.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using Xunit.Abstractions;

namespace HaikuApi.Tests.UnitTests
{
    public class CreatorServiceTests
    {
        private readonly CreatorController _controller;
        private readonly Mock<ICreatorService> _mockService = new();
        private readonly Mock<IMapper> _mockMapper = new();
        private readonly Mock<ILogger<CreatorController>> _mockLogger = new();
        private readonly ITestOutputHelper _output;

        public CreatorServiceTests(ITestOutputHelper output)
        {
            _output = output;

            _controller = new CreatorController(_mockService.Object, _mockMapper.Object, _mockLogger.Object);

        }

        [Fact]
        public async Task GetCreatorAsync_ReturnCreator()
        {
            long creatorId = 1;

            var expectedCreatorDto = new CreatorDto
            {
                Id = creatorId,
                Name = "Unknown",
                Bio = "No Bio.",
            };

            var existingCreator = new CreatorItem
            {
                Id = creatorId,
                Name = "Unknown",
                Bio = "No Bio.",
            };

            _mockService.Setup(service => service.CreatorExistsAsync(creatorId))
                .ReturnsAsync(true);

            _mockService.Setup(service => service.GetCreatorByIdAsync(creatorId))
                    .ReturnsAsync(existingCreator);

            _mockMapper.Setup(mapper => mapper.Map<CreatorDto>(It.IsAny<CreatorItem>()))
                    .Returns(expectedCreatorDto);

            var result = await _controller.GetCreatorAsync(creatorId);

            var actionResult = Assert.IsType<ActionResult<CreatorDto>>(result);
            var createdResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            _output.WriteLine($"Method: GET | Endpoint: /api/Creator/{creatorId} | Status Code: {createdResult.StatusCode} | " +
                           $"Should: Status = 200");

            Assert.Equal(expectedCreatorDto, createdResult.Value);
        }

        [Fact]
        public async Task CreateCreatorAsync_AddCreator()
        {
            var newCreatorDto = new CreatorDto
            {
                Name = "Unknown",
                Bio = "No Bio.",
            };

            var newCreator = new CreatorItem
            {
                Name = "Unknown",
                Bio = "No Bio.",
            };

            _mockService.Setup(service => service.CreateCreatorAsync(It.IsAny<CreatorItem>()))
                    .ReturnsAsync(newCreator);

            _mockMapper.Setup(mapper => mapper.Map<CreatorDto>(It.IsAny<CreatorItem>()))
                    .Returns(newCreatorDto);

            var result = await _controller.PostCreatorAsync(newCreatorDto);

            var actionResult = Assert.IsType<ActionResult<CreatorDto>>(result);
            var createdResult = Assert.IsType<CreatedAtRouteResult>(actionResult.Result);

            _output.WriteLine($"Method: POST | Endpoint: /api/Creator | " +
                $"Status Code: {createdResult.StatusCode} | Expected Status Code: 201");

            Assert.Equal("CreatorDetails", createdResult.RouteName);
            Assert.Equal(newCreatorDto, createdResult.Value);
        }

        [Fact]
        public async Task UpdateCreatorAsync_UpdateCreator()
        {
            long creatorId = 1;

            var existingCreator = new CreatorItem
            {
                Id = creatorId,
                Name = "Name",
                Bio = "Bio",
            };
            var updatedCreator = new CreatorItem
            {
                Id = creatorId,
                Name = "Updated Name",
                Bio = "Updated Bio",
            };
            var updatedCreatorDto = new CreatorDto
            {
                Id = creatorId,
                Name = "Name",
                Bio = "Updated Bio",
            };

            _mockService.Setup(service => service.CreatorExistsAsync(creatorId))
                .ReturnsAsync(true);

            _mockService.Setup(service => service.GetCreatorByIdAsync(creatorId))
                .ReturnsAsync(existingCreator);

            _mockService.Setup(service => service.UpdateCreatorAsync(It.IsAny<CreatorItem>()))
                .Returns(Task.CompletedTask);

            _mockMapper.Setup(mapper => mapper.Map<CreatorDto>(It.IsAny<CreatorItem>()))
                .Returns(updatedCreatorDto);


            var result = await _controller.PutCreatorAsync(creatorId, updatedCreatorDto);

            var noContentResult = Assert.IsType<NoContentResult>(result);

            _output.WriteLine($"Method: PUT | Endpoint: /api/Creator/{creatorId} | Status Code: {noContentResult.StatusCode} | " +
                           $"Should: Status = 204");

            Assert.Equal((int)HttpStatusCode.NoContent, noContentResult.StatusCode);
        }

        [Fact]
        public async Task DeleteCreatorAsync_DeleteCreator()
        {
            long creatorId = 1;

            _mockService.Setup(service => service.CreatorExistsAsync(creatorId))
                .ReturnsAsync(true);

            _mockService.Setup(service => service.DeleteCreatorAsync(creatorId))
                .Returns(Task.CompletedTask);

            var result = await _controller.DeleteCreatorAsync(creatorId);

            var noContentResult = Assert.IsType<NoContentResult>(result);

            _output.WriteLine($"Method: DELETE | Endpoint: /api/Creator/{creatorId} | Status Code: {noContentResult.StatusCode} | " +
                           $"Should: Status = 204");

            Assert.Equal((int)HttpStatusCode.NoContent, noContentResult.StatusCode);
        }
    }
}
