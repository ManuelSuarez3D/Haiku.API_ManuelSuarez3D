using Haiku.API.Models;
using Haiku.API.Repositories;

namespace Haiku.API.Services
{
    public class HaikuService : IHaikuService
    {
        private readonly IHaikuRepository _repository;
        private readonly ILogger<HaikuService> _logger;

        public HaikuService(IHaikuRepository repository, ILogger<HaikuService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<HaikuItem>> GetHaikusAsync(int page, int pageSize)
        {
            try
            {
                _logger.LogInformation($"Fetching haikus for page {page} with page size {pageSize} in Service.");
                return await _repository.GetHaikusAsync(page, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching haikus for page {page} with page size {pageSize} in Service.");
                throw;
            }
        }

        public async Task<int> GetTotalHaikusAsync()
        {
            try
            {
                var totalHaikus = await _repository.GetTotalHaikusAsync();
                _logger.LogInformation($"Total number of haikus fetched successfully: {totalHaikus} total in Service.");
                return totalHaikus;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the total number of haikus in Service.");
                throw;
            }
        }

        public async Task<HaikuItem> GetHaikuByIdAsync(long id)
        {
            try
            {
                var haiku = await _repository.GetHaikuByIdAsync(id);
                if (haiku == null)
                {
                    _logger.LogWarning($"Haiku with ID: {id} was not found in Service.");
                    return null;
                }

                _logger.LogInformation($"Haiku fetched successfully with ID: {id} in Service.");
                return haiku;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching haiku with ID: {id} in Service.");
                throw;
            }
        }

        public async Task<HaikuItem> CreateHaikuAsync(HaikuItem haiku)
        {
            if (haiku == null)
            {
                _logger.LogError("Haiku can't be Null in Service.");
                throw new ArgumentNullException(nameof(haiku), "Haiku can't be Null in Service.");
            }

            try
            {
                return await _repository.CreateHaikuAsync(haiku);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while creating haiku in Service.");
                throw;
            }
        }

        public async Task UpdateHaikuAsync(HaikuItem haiku)
        {
            if (haiku == null)
            {
                _logger.LogError("Haiku can't be Null in Service.");
                throw new ArgumentNullException(nameof(haiku), "Haiku can't be Null in Service.");
            }

            try
            {
                await _repository.UpdateHaikuAsync(haiku);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating haiku with ID: {haiku.Id} in Service.");
                throw;
            }
        }

        public async Task DeleteHaikuAsync(long id)
        {
            try
            {
                var haiku = await _repository.GetHaikuByIdAsync(id);
                if (haiku == null)
                {
                    _logger.LogWarning($"Haiku with ID: {id} was not found for deletion in Service.");
                    return;
                }

                await _repository.DeleteHaikuAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting haiku with ID: {id} in Service.");
                throw;
            }
        }

        public async Task<bool> HaikuExistsAsync(long id)
        {
            try
            {
                return await _repository.HaikuExistsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while checking existence of haiku with ID: {id} in Service.");
                throw;
            }
        }
    }
}
