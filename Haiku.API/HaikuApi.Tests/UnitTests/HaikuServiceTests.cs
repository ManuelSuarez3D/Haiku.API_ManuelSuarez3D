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
    public class HaikuServiceTests
    {
        private readonly HaikuController _controller;
        private readonly Mock<IHaikuService> _mockHaikuService = new();
        private readonly Mock<IMapper> _mockMapper = new();
        private readonly Mock<ILogger<HaikuController>> _mockLogger = new();
        private readonly ITestOutputHelper _output;

        public HaikuServiceTests(ITestOutputHelper output)
        {
            _output = output;

            _controller = new HaikuController(_mockHaikuService.Object, _mockMapper.Object, _mockLogger.Object);

        }

        [Fact]
        public async Task GetHaikuAsync_ReturnHaiku()
        {
            long haikuId = 1;

            var expectedHaikuDto = new HaikuDto
            {
                Id = haikuId,
                Title = "Echoes of Spring.",
                LineOne = "Whispers in the breeze,",
                LineTwo = "Cherry blossoms kiss the sky,",
                LineThree = "Dreams of hope arise.",
                CreatorId = 1
            };

            var existingHaiku = new HaikuItem
            {
                Id = haikuId,
                Title = "Echoes of Spring.",
                LineOne = "Whispers in the breeze,",
                LineTwo = "Cherry blossoms kiss the sky,",
                LineThree = "Dreams of hope arise.",
                CreatorId = 1
            };

            _mockHaikuService.Setup(service => service.HaikuExistsAsync(haikuId))
                .ReturnsAsync(true);

            _mockHaikuService.Setup(service => service.GetHaikuByIdAsync(haikuId))
                    .ReturnsAsync(existingHaiku);

            _mockMapper.Setup(mapper => mapper.Map<HaikuDto>(It.IsAny<HaikuItem>()))
                    .Returns(expectedHaikuDto);

            var result = await _controller.GetHaikuAsync(haikuId);

            var actionResult = Assert.IsType<ActionResult<HaikuDto>>(result);
            var createdResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            _output.WriteLine($"Method: GET | Endpoint: /api/Haiku/{haikuId} | Status Code: {createdResult.StatusCode} | " +
                           $"Should: Status = 200");

            Assert.Equal(expectedHaikuDto, createdResult.Value);
        }

        [Fact]
        public async Task CreateHaikuAsync_AddHaiku()
        {
            var newHaikuDto = new HaikuDto
            {
                Title = "An Old Silent Pond",
                LineOne = "An old silent pond...",
                LineTwo = "A frog jumps into the pond,",
                LineThree = "splash! Silence again.",
                CreatorId = 1
            };

            var newHaiku = new HaikuItem
            {
                Title = "An Old Silent Pond",
                LineOne = "An old silent pond...",
                LineTwo = "A frog jumps into the pond,",
                LineThree = "splash! Silence again.",
                CreatorId = 1
            };

            _mockHaikuService.Setup(service => service.CreateHaikuAsync(It.IsAny<HaikuItem>()))
                    .ReturnsAsync(newHaiku);

            _mockMapper.Setup(mapper => mapper.Map<HaikuDto>(It.IsAny<HaikuItem>()))
                    .Returns(newHaikuDto);

            var result = await _controller.PostHaikuAsync(newHaikuDto);

            var actionResult = Assert.IsType<ActionResult<HaikuDto>>(result);
            var createdResult = Assert.IsType<CreatedAtRouteResult>(actionResult.Result);

            _output.WriteLine($"Method: POST | Endpoint: /api/Haiku | " +
                $"Status Code: {createdResult.StatusCode} | Expected Status Code: 201");

            Assert.Equal("HaikuDetails", createdResult.RouteName);
            Assert.Equal(newHaikuDto, createdResult.Value);
        }
        

        [Fact]
        public async Task UpdateHaikuAsync_UpdateHaiku()
        {
            long haikuId = 1;

            var existingHaiku = new HaikuItem
            {
                Id = haikuId,
                Title = "Echoes of Hope.",
                LineOne = "Whispers in the breeze,",
                LineTwo = "Cherry blossoms kiss the sky,",
                LineThree = "Dreams of hope arise.",
                CreatorId = 1
            };
            var updatedHaiku = new HaikuItem
            {
                Id = haikuId,
                Title = "Echoes of Spring.",
                LineOne = "Whispers in the breeze,",
                LineTwo = "Cherry blossoms kiss the sky,",
                LineThree = "Dreams of hope arise.",
                CreatorId = 1
            };
            var updatedHaikuDto = new HaikuDto
            {
                Id = haikuId,
                Title = "Echoes of Spring.",
                LineOne = "Whispers in the breeze,",
                LineTwo = "Cherry blossoms kiss the sky,",
                LineThree = "Dreams of hope arise.",
                CreatorId = 1
            };

            _mockHaikuService.Setup(service => service.HaikuExistsAsync(haikuId))
                .ReturnsAsync(true);

            _mockHaikuService.Setup(service => service.GetHaikuByIdAsync(haikuId))
                .ReturnsAsync(existingHaiku);

            _mockHaikuService.Setup(service => service.UpdateHaikuAsync(It.IsAny<HaikuItem>()))
                .Returns(Task.CompletedTask);

            _mockMapper.Setup(mapper => mapper.Map<HaikuDto>(It.IsAny<HaikuItem>()))
                .Returns(updatedHaikuDto);

            var result = await _controller.PutHaikuAsync(haikuId, updatedHaikuDto);

            var noContentResult = Assert.IsType<NoContentResult>(result);

            _output.WriteLine($"Method: PUT | Endpoint: /api/Haiku/{haikuId} | Status Code: {noContentResult.StatusCode} | " +
                           $"Should: Status = 204");

            Assert.Equal((int)HttpStatusCode.NoContent, noContentResult.StatusCode);
        }

        [Fact]
        public async Task DeleteHaikuAsync_DeleteHaiku()
        {
            long haikuId = 1;

            _mockHaikuService.Setup(service => service.HaikuExistsAsync(haikuId))
                .ReturnsAsync(true);

            _mockHaikuService.Setup(service => service.DeleteHaikuAsync(haikuId))
                .Returns(Task.CompletedTask);

            var result = await _controller.DeleteHaikuAsync(haikuId);
            var noContentResult = Assert.IsType<NoContentResult>(result);

            _output.WriteLine($"Method: DELETE | Endpoint: /api/Haiku/{haikuId} | Status Code: {noContentResult.StatusCode} | " +
                           $"Should: Status = 204");

            Assert.Equal((int)HttpStatusCode.NoContent, noContentResult.StatusCode);
        }
    }
}