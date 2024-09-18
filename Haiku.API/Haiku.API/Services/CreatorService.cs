using Haiku.API.Models;
using Haiku.API.Repositories;

namespace Haiku.API.Services
{
    public class CreatorService : ICreatorService
    {
        private readonly ICreatorRepository _repository;
        private readonly ILogger<CreatorService> _logger;

        public CreatorService(ICreatorRepository repository, ILogger<CreatorService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<CreatorItem>> GetCreatorsAsync(int page, int pageSize)
        {
            try
            {
                _logger.LogInformation($"Fetching creators for page {page} with page size {pageSize} in Service.");
                var creators = await _repository.GetCreatorsAsync(page, pageSize);
                _logger.LogInformation($"Successfully fetched {creators.Count()} creators for page {page} in Service.");
                return creators;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching creators for page {page} with page size {pageSize} in Service.");
                throw;
            }
        }

        public async Task<int> GetTotalCreatorsAsync()
        {
            try
            {
                var totalCreators = await _repository.GetTotalCreatorsAsync();
                _logger.LogInformation($"Total number of creators fetched successfully: {totalCreators} total in Service.");
                return totalCreators;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the total number of creators in Service.");
                throw;
            }
        }

        public async Task<CreatorItem> GetCreatorByIdAsync(long id)
        {
            try
            {
                var creator = await _repository.GetCreatorByIdAsync(id);

                if (creator == null)
                {
                    _logger.LogWarning($"Creator with ID: {id} was not found in Service.");
                    return null;
                }

                _logger.LogInformation($"Creator fetched successfully with ID: {id} in Service.");
                return creator;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching creator with ID: {id} in Service.");
                throw;
            }
        }

        public async Task<CreatorItem> CreateCreatorAsync(CreatorItem creator)
        {
            if (creator == null)
            {
                _logger.LogError("Creator can't be Null in Service.");
                throw new ArgumentNullException(nameof(creator), "Creator can't be Null in Service.");
            }

            try
            {
                var createdCreator = await _repository.AddCreatorAsync(creator);
                _logger.LogInformation($"Creator created successfully with ID: {createdCreator.Id} in Service.");
                return createdCreator;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating creator in Service.");
                throw;
            }
        }

        public async Task UpdateCreatorAsync(CreatorItem creator)
        {
            if (creator == null)
            {
                _logger.LogError("Creator can't be Null in Service.");
                throw new ArgumentNullException(nameof(creator), "Creator can't be Null in Service.");
            }

            try
            {
                await _repository.UpdateCreatorAsync(creator);
                _logger.LogInformation($"Creator updated successfully with ID: {creator.Id} in Service.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating creator with ID: {creator.Id} in Service.");
                throw;
            }
        }

        public async Task DeleteCreatorAsync(long id)
        {
            try
            {
                await _repository.DeleteCreatorAsync(id);
                _logger.LogInformation($"Creator deleted successfully with ID: {id} in Service.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting creator with ID: {id} in Service.");
                throw;
            }
        }

        public async Task<bool> CreatorExistsAsync(long id)
        {
            try
            {
                return await _repository.CreatorExistsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while checking existence of creator with ID: {id} in Service.");
                throw;
            }
        }
    }
}
