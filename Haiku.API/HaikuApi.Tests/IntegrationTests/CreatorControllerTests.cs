using System.Text;
using FluentAssertions;
using Haiku.API.Dtos;
using Haiku.API.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace HaikuApi.Tests.IntegrationTests
{
    public class CreatorControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly ITestOutputHelper _output;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public CreatorControllerTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
            _output = output;
        }

        [Fact]
        public async Task GetAllCreators_SuccessStatusCode()
        {
            ResetDatabase();

            var response = await _httpClient.GetAsync("/api/Creator");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.MediaType.Should().Be("application/json");

            var responseString = await response.Content.ReadAsStringAsync();
            var returnedCreators = JsonConvert.DeserializeObject<List<CreatorDto>>(responseString);

            _output.WriteLine($"Method: GET | Endpoint: /api/Creator | Status Code: {response.StatusCode} | " +
                $"Should: Status = OK, NotBeNull, HaveCountGreaterThan(0)");

            returnedCreators.Should().NotBeNull();
            returnedCreators.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetCreatorById_SuccessStatusCode()
        {
            ResetDatabase();

            var testId = 1;

            var response = await _httpClient.GetAsync($"/api/Creator/{testId}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.MediaType.Should().Be("application/json");

            var responseString = await response.Content.ReadAsStringAsync();
            var returnedCreator = JsonConvert.DeserializeObject<CreatorDto>(responseString);

            _output.WriteLine($"Method: GET | Endpoint: /api/Creator/{testId} | Status Code: {response.StatusCode} | " +
                $"Should: Status = OK, NotBeNull, Name = 'Unknown', Bio = 'No Bio'");

            returnedCreator.Should().NotBeNull();
            returnedCreator.Name.Should().Be("Unknown");
            returnedCreator.Bio.Should().Be("No bio.");
        }
        [Fact]
        public async Task GetCreatorById_FailStatusCode()
        {
            ResetDatabase();

            var testId = 414214241;

            var response = await _httpClient.GetAsync($"/api/Creator/{testId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseString = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Method: GET | Endpoint: /api/Creator/{testId} | Status Code: {response.StatusCode} | " +
                $"Should: Status = NotFound, Contain = 'Creator with ID {testId} not found'");

            responseString.Should().Contain($"Creator with ID {testId} not found");
        }

        [Fact]
        public async Task PostCreator_SuccessStatusCode()
        {
            ResetDatabase();

            var newCreatorDto = new CreatorDto
            {
                Name = "Test Creator",
                Bio = "This is a bio for the Test Creator Integration."
            };

            var content = GetStringContent(newCreatorDto);

            _output.WriteLine("Sending POST request to /api/Creator.");
            var response = await _httpClient.PostAsync("/api/Creator", content);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Content.Headers.ContentType.MediaType.Should().Be("application/json");

            var responseString = await response.Content.ReadAsStringAsync();
            var returnedCreator = JsonConvert.DeserializeObject<CreatorDto>(responseString);

            _output.WriteLine($"Method: POST | Endpoint: /api/Creator | Status Code: {response.StatusCode} | " +
                $"Should: Status = Created, NotBeNull, " +
                $"Name = 'Test Creator', " +
                $"Bio = 'This is a bio for the Test Creator Integration.'"
                );

            returnedCreator.Should().NotBeNull();
            returnedCreator.Name.Should().Be("Test Creator");
            returnedCreator.Bio.Should().Be("This is a bio for the Test Creator Integration.");
        }

        [Fact]
        public async Task PostCreator_ErrorStatusCode()
        {
            ResetDatabase();

            var creatorDto = new CreatorDto
            {
                Name = "",
                Bio = ""
            };

            var content = GetStringContent(creatorDto);

            var response = await _httpClient.PostAsync("/api/Creator", content);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Method: POST | Endpoint: /api/Creator | Status Code: {response.StatusCode} | " +
                $"Should: Status = BadRequest, Contain = 'The Name field is required.'");

            responseString.Should().Contain("The Name field is required.");
        }

        [Fact]
        public async Task PutCreatorById_SuccessStatusCode()
        {
            ResetDatabase();

            var testId = 1;

            var creatorDto = new CreatorDto
            {
                Name = "Unknown",
                Bio = "This is a bio for the Unknown Creator, Put."
            };

            var content = GetStringContent(creatorDto);

            var response = await _httpClient.PutAsync($"/api/Creator/{testId}", content);

            _output.WriteLine($"Method: PUT | Endpoint: /api/Creator/{testId} | Status Code: {response.StatusCode} | " +
                $"Should: Status = NoContent");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        [Fact]
        public async Task PutCreatorById_ErrorStatusCode()
        {
            ResetDatabase();

            var testId = 1;

            var creatorDto = new CreatorDto
            {
                Name = "",
                Bio = "This is a bio for the Test Creator, Put failure."
            };

            var content = GetStringContent(creatorDto);

            var response = await _httpClient.PutAsync($"/api/Creator/{testId}", content);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Method: PUT | Endpoint: /api/Creator/{testId} | Status Code: {response.StatusCode} | " +
                $"Should: Status = BadRequest, Contain = 'The Name field is required.'");

            responseString.Should().Contain("The Name field is required.");
        }

        [Fact]
        public async Task DeleteCreatorById_SuccessStatusCode()
        {
            ResetDatabase();

            var testId = 1;

            var response = await _httpClient.DeleteAsync($"/api/Creator/{testId}");

            _output.WriteLine($"Method: DELETE | Endpoint: /api/Creator/{testId} | Status Code: {response.StatusCode} | " +
                $"Should: Status = NoContent");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        [Fact]
        public async Task DeleteCreatorById_ErrorStatusCode()
        {
            ResetDatabase();

            var testId = 42141241241;

            var response = await _httpClient.DeleteAsync($"/api/Creator/{testId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseString = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Method: DELETE | Endpoint: /api/Creator/{testId} | Status Code: {response.StatusCode} | " +
                $"Should: Status = NotFound, Contain = 'Creator with ID {testId} not found'");

            responseString.Should().Contain($"Creator with ID {testId} not found");
        }

        private void ResetDatabase()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<HaikuAPIContext>();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }
        private StringContent GetStringContent(object obj)
        {
            return new StringContent(
                JsonConvert.SerializeObject(obj),
                Encoding.UTF8,
                "application/json");
        }
    }
}

