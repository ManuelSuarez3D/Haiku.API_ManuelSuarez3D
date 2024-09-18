using AutoMapper;
using Haiku.API.Dtos;
using Haiku.API.Models;
using Haiku.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Haiku.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HaikuController : ControllerBase
    {
        private readonly IHaikuService _haikuService;
        private readonly IMapper _mapper;
        private readonly ILogger<HaikuController> _logger;

        public HaikuController(IHaikuService haikuService, IMapper mapper, ILogger<HaikuController> logger)
        {
            _haikuService = haikuService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHaikusAsync(int page = 1, int pageSize = 10)
        {
            try
            {
                var haikus = await _haikuService.GetHaikusAsync(page, pageSize);
                var haikuDtos = _mapper.Map<IEnumerable<HaikuDto>>(haikus);
                var totalHaikus = await _haikuService.GetTotalHaikusAsync();

                var paginationMetadata = new
                {
                    totalCount = totalHaikus,
                    pageSize = pageSize,
                    currentPage = page,
                    totalPages = (int)Math.Ceiling((double)totalHaikus / pageSize)
                };

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata, options));

                _logger.LogInformation($"Haikus fetched successfully in Controller.");
                return Ok(haikuDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching haikus in Controller.");
                return StatusCode(500, $"An unexpected error occurred while fetching haikus.");
            }
        }

        [HttpGet("{id}", Name = "HaikuDetails")]
        public async Task<ActionResult<HaikuDto>> GetHaikuAsync(long id)
        {
            try
            {
                if (!await _haikuService.HaikuExistsAsync(id))
                    return NotFound($"Haiku with ID {id} not found.");

                var haiku = await _haikuService.GetHaikuByIdAsync(id);
                var haikuDto = _mapper.Map<HaikuDto>(haiku);

                _logger.LogInformation($"Haiku fetched successfully with ID: {haikuDto.Id} in Controller.");
                return Ok(haikuDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching haiku with ID: {id} in Controller.");
                return StatusCode(500, $"An unexpected error occurred while fetching haiku with ID: {id}.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<HaikuDto>> PostHaikuAsync(HaikuDto haikuDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var haiku = _mapper.Map<HaikuItem>(haikuDto);

                var newHaiku = await _haikuService.CreateHaikuAsync(haiku);
                var newHaikuDto = _mapper.Map<HaikuDto>(newHaiku);

                _logger.LogInformation($"Haiku created successfully with ID: {newHaikuDto.Id} in Controller.");
                return CreatedAtRoute("HaikuDetails", new { id = newHaikuDto.Id }, newHaikuDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while creating haiku in Controller.");
                return StatusCode(500, $"An unexpected error occurred while creating haiku.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutHaikuAsync(long id, [FromBody] HaikuDto haikuDto)
        {
            try
            {
                if (!ModelState.IsValid)         
                    return BadRequest(ModelState);

                if (!await _haikuService.HaikuExistsAsync(id))
                    return NotFound($"Haiku with ID {id} not found.");

                var existingHaiku = await _haikuService.GetHaikuByIdAsync(id);

                existingHaiku.Title = haikuDto.Title;
                existingHaiku.LineOne = haikuDto.LineOne;
                existingHaiku.LineTwo = haikuDto.LineTwo;
                existingHaiku.LineThree = haikuDto.LineThree;
                existingHaiku.CreatorId = haikuDto.CreatorId;

                await _haikuService.UpdateHaikuAsync(existingHaiku);

                _logger.LogInformation($"Haiku updated successfully with ID: {existingHaiku.Id} in Controller.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating haiku with ID: {id} in Controller.");
                return StatusCode(500, $"An unexpected error occurred while updating haiku with ID: {id}.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHaikuAsync(long id)
        {
            try
            {
                if (!await _haikuService.HaikuExistsAsync(id))
                    return NotFound($"Haiku with ID {id} not found.");

                await _haikuService.DeleteHaikuAsync(id);

                _logger.LogInformation($"Haiku deleted successfully with ID: {id} in Controller.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting haiku with ID: {id} in Controller.");
                return StatusCode(500, $"An unexpected error occurred while deleting haiku with ID: {id}.");
            }
        }
    }
}
