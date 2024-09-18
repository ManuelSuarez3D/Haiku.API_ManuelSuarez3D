using FluentAssertions;
using Haiku.API.Dtos;
using Haiku.API.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Xunit.Abstractions;

namespace HaikuApi.Tests.IntegrationTests
{
    public class HaikuControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly ITestOutputHelper _output;
        private readonly CustomWebApplicationFactory<Program> _factory;
    
        public HaikuControllerTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
            _output = output;
        }

        [Fact]
        public async Task GetAllHaikus_SuccessStatusCode()
        {

            ResetDatabase();

            var response = await _httpClient.GetAsync("/api/Haiku");
    
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.MediaType.Should().Be("application/json");
    
            var responseString = await response.Content.ReadAsStringAsync();
            var returnedCreators = JsonConvert.DeserializeObject<List<HaikuDto>>(responseString);

            _output.WriteLine($"Method: GET | Endpoint: /api/Haiku | Status Code: {response.StatusCode} | " +
                $"Should: Status = OK, NotBeNull, HaveCountGreaterThan(0)");

            returnedCreators.Should().NotBeNull();
            returnedCreators.Should().HaveCountGreaterThan(0);
        }
    
        [Fact]
        public async Task GetHaikuById_SuccessStatusCode()
        {
            ResetDatabase();

            var testId = 1;
    
            var response = await _httpClient.GetAsync($"/api/Haiku/{testId}");
    
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.MediaType.Should().Be("application/json");
    
            var responseString = await response.Content.ReadAsStringAsync();
            var returnedCreator = JsonConvert.DeserializeObject<HaikuDto>(responseString);

            _output.WriteLine($"Method: GET | Endpoint: /api/Haiku | Status Code: {response.StatusCode} |" +
                $" Should: Status = OK, NotBeNull, " +
                $"Title = 'An Old Silent Pond', " +
                $"LineOne = 'An old silent pond...', " +
                $"LineTwo = 'A frog jumps into the pond', " +
                $"LineThree = 'splash! Silence again.', " +
                $"CreatorId = 2"
                );

            returnedCreator.Should().NotBeNull();
            returnedCreator.Title.Should().Be("An Old Silent Pond");
            returnedCreator.LineOne.Should().Be("An old silent pond...");
            returnedCreator.LineTwo.Should().Be("A frog jumps into the pond,");
            returnedCreator.LineThree.Should().Be("splash! Silence again.");
            returnedCreator.CreatorId.Should().Be(2);
        }
        [Fact]
        public async Task GetHaikuById_FailStatusCode()
        {
            ResetDatabase();

            var testId = 414214241;
    
            var response = await _httpClient.GetAsync($"/api/Haiku/{testId}");
    
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseString = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Method: GET | Endpoint: /api/Haiku/{testId} | Status Code: {response.StatusCode} | " +
                $"Should: Status = NotFound, Contain = 'Haiku with ID {testId} not found'");

            responseString.Should().Contain($"Haiku with ID {testId} not found");
        }

        [Fact]
        public async Task PostHaiku_SuccessStatusCode()
        {
            ResetDatabase();

            var newHaikuDto = new HaikuDto
            {
                Title = "Echoes of Spring.",
                LineOne = "Whispers in the breeze,",
                LineTwo = "Cherry blossoms kiss the sky,",
                LineThree = "Dreams of hope arise.",
                CreatorId = 1
            };

            var content = GetStringContent(newHaikuDto);

            var response = await _httpClient.PostAsync("/api/Haiku", content);
    
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Content.Headers.ContentType.MediaType.Should().Be("application/json");
    
            var responseString = await response.Content.ReadAsStringAsync();
            var returnedCreator = JsonConvert.DeserializeObject<HaikuDto>(responseString);

            _output.WriteLine($"Method: POST | Endpoint: /api/Creator | Status Code: {response.StatusCode} | " +
                $"Should: Status = Created, NotBeNull, " +
                $"Title = 'Echoes of Spring.', " +
                $"LineOne = 'Whispers in the breeze,', " +
                $"LineTwo = 'Cherry blossoms kiss the sky,', " +
                $"LineThree = 'Dreams of hope arise.', " +
                $"CreatorId = 1"
            );

            returnedCreator.Should().NotBeNull();
            returnedCreator.Title.Should().Be("Echoes of Spring.");
            returnedCreator.LineOne.Should().Be("Whispers in the breeze,");
            returnedCreator.LineTwo.Should().Be("Cherry blossoms kiss the sky,");
            returnedCreator.LineThree.Should().Be("Dreams of hope arise.");
            returnedCreator.CreatorId.Should().Be(1);
        }
    
        [Fact]
        public async Task PostHaiku_ErrorStatusCode()
        {
            ResetDatabase();

            var newHaikuDto = new HaikuDto
            {
                Title = "Echoes of Spring.",
                LineOne = "",
                LineTwo = "",
                LineThree = "",
                CreatorId = 1
            };

            var content = GetStringContent(newHaikuDto);

            var response = await _httpClient.PostAsync("/api/Haiku", content);
    
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Method: POST | Endpoint: /api/Haiku | Status Code: {response.StatusCode} | " +
                $"Should: Status = BadRequest, " +
                $"Contain = 'The LineOne field is required.'" +
                $"'The LineTwo field is required.'" +
                $"'The LineThree field is required.'" +
                $"'Must be five syllables'" +
                $"Must be seven syllables"
            );

            responseString.Should().Contain("The LineOne field is required.");
            responseString.Should().Contain("The LineTwo field is required.");
            responseString.Should().Contain("The LineThree field is required.");
            responseString.Should().Contain("Must be five syllables");
            responseString.Should().Contain("Must be seven syllables");
        }
    
        [Fact]
        public async Task PutHaikuById_SuccessStatusCode()
        {
            ResetDatabase();

            var testId = 1;

            var haikuDto = new HaikuDto
            {
                Title = "Echoes of Hope.",
                LineOne = "Whispers in the breeze,",
                LineTwo = "Cherry blossoms kiss the sky,",
                LineThree = "Dreams of hope arise.",
                CreatorId = 1
            };

            var content = GetStringContent(haikuDto);

            var response = await _httpClient.PutAsync($"/api/Haiku/{testId}", content);

            _output.WriteLine($"Method: PUT | Endpoint: /api/Haiku/{testId} | Status Code: {response.StatusCode} | " +
                $"Should: Status = NoContent");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        [Fact]
        public async Task PutHaikuById_ErrorStatusCode()
        {
            ResetDatabase();

            var testId = 1;

            var haikuDto = new HaikuDto
            {
                Title = "",
                LineOne = "",
                LineTwo = "",
                LineThree = "",
                CreatorId = 1
            };

            var content = GetStringContent(haikuDto);

            var response = await _httpClient.PutAsync($"/api/Haiku/{testId}", content);
    
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Method: PUT | Endpoint: /api/Haiku/{testId} | Status Code: {response.StatusCode} | " +
                $"Should: Status = BadRequest, " +
                $"Contain = 'The Title field is required.'" +
                $"'The LineOne field is required.'" +
                $"'The LineTwo field is required.'" +
                $"'The LineThree field is required.'" +
                $"'Must be five syllables'" +
                $"Must be seven syllables"
            );

            responseString.Should().Contain("The Title field is required.");
            responseString.Should().Contain("The LineOne field is required.");
            responseString.Should().Contain("The LineTwo field is required.");
            responseString.Should().Contain("The LineThree field is required.");
            responseString.Should().Contain("Must be five syllables");
            responseString.Should().Contain("Must be seven syllables");
        }
    
        [Fact]
        public async Task DeleteHaikuById_SuccessStatusCode()
        {
            ResetDatabase();

            var testId = 1;
    
            var response = await _httpClient.DeleteAsync($"/api/Haiku/{testId}");

            _output.WriteLine($"Method: DELETE | Endpoint: /api/Haiku/{testId} | Status Code: {response.StatusCode} | " +
                $"Should: Status = NoContent");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        [Fact]
        public async Task DeleteHaikuById_ErrorStatusCode()
        {
            ResetDatabase();

            var testId = 42141241241;
    
            var response = await _httpClient.DeleteAsync($"/api/Haiku/{testId}");
    
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseString = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Method: DELETE | Endpoint: /api/Haiku/{testId} | Status Code: {response.StatusCode} | " +
                $"Should: Status = NotFound, Contain = 'Haiku with ID {testId} not found'");

            responseString.Should().Contain($"Haiku with ID {testId} not found");
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
