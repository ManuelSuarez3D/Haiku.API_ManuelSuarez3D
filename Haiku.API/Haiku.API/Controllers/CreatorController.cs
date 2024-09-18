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
    public class CreatorController : ControllerBase
    {
        private readonly ICreatorService _creatorService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreatorController> _logger;

        public CreatorController(ICreatorService creatorService, IMapper mapper, ILogger<CreatorController> logger)
        {
            _creatorService = creatorService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCreatorsAsync(int page = 1, int pageSize = 10)
        {
            try
            {
                var creators = await _creatorService.GetCreatorsAsync(page, pageSize);
                var creatorDtos = _mapper.Map<IEnumerable<CreatorDto>>(creators);
                var totalCreators = await _creatorService.GetTotalCreatorsAsync();

                var paginationMetadata = new
                {
                    totalCount = totalCreators,
                    pageSize = pageSize,
                    currentPage = page,
                    totalPages = (int)Math.Ceiling((double)totalCreators / pageSize)
                };

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata, options));

                _logger.LogInformation($"Creators fetched successfully in Controller.");
                return Ok(creatorDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching creators in Controller.");
                return StatusCode(500, $"An unexpected error occurred while fetching creators. {ex}");
            }
        }

        [HttpGet("{id}", Name = "CreatorDetails")]
        public async Task<ActionResult<CreatorDto>> GetCreatorAsync(long id)
        {
            try
            {
                if (!await _creatorService.CreatorExistsAsync(id))
                    return NotFound($"Creator with ID {id} not found.");

                var creator = await _creatorService.GetCreatorByIdAsync(id);
                var creatorDto = _mapper.Map<CreatorDto>(creator);

                _logger.LogInformation($"Creator fetched successfully with ID: {creatorDto.Id} in Controller.");
                return Ok(creatorDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching creator with ID: {id} in Controller.");
                return StatusCode(500, $"An unexpected error occurred while fetching creator with ID: {id}.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CreatorDto>> PostCreatorAsync(CreatorDto creatorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var creator = _mapper.Map<CreatorItem>(creatorDto);
                var newCreator = await _creatorService.CreateCreatorAsync(creator);
                var newCreatorDto = _mapper.Map<CreatorDto>(newCreator);

                _logger.LogInformation($"Creator created successfully with ID: {newCreatorDto.Id} in Controller.");
                return CreatedAtRoute("CreatorDetails", new { id = newCreatorDto.Id }, newCreatorDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating creator in Controller. Exception: {Message}, StackTrace: {StackTrace}", ex.Message, ex.StackTrace);
                _logger.LogError(ex, $"An error occurred while creating creator in Controller.");
                return StatusCode(500, $"An unexpected error occurred while creating creator.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCreatorAsync(long id, [FromBody] CreatorDto creatorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!await _creatorService.CreatorExistsAsync(id))
                    return NotFound($"Creator with ID {id} not found.");

                var existingCreator = await _creatorService.GetCreatorByIdAsync(id);

                existingCreator.Name = creatorDto.Name ?? existingCreator.Name;
                existingCreator.Bio = creatorDto.Bio ?? existingCreator.Bio;

                await _creatorService.UpdateCreatorAsync(existingCreator);

                _logger.LogInformation($"Creator updated successfully with ID: {existingCreator.Id} in Controller.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating creator with ID: {id} in Controller.");
                return StatusCode(500, $"An unexpected error occurred while updating creator with ID: {id}.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCreatorAsync(long id)
        {
            try
            {
                if (!await _creatorService.CreatorExistsAsync(id))
                    return NotFound($"Creator with ID {id} not found.");

                await _creatorService.DeleteCreatorAsync(id);

                _logger.LogInformation($"Creator deleted successfully with ID: {id} in Controller.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting haiku with ID: {id} in Controller.");
                return StatusCode(500, $"An unexpected error occurred while deleting creator with ID: {id}.");
            }
        }

    }
}
