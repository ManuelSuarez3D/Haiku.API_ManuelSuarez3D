using Haiku.API.Models;

namespace Haiku.API.Services
{
    public interface ICreatorService
    {
        Task<IEnumerable<CreatorItem>> GetCreatorsAsync(int page, int pageSize);
        Task<int> GetTotalCreatorsAsync();
        Task<CreatorItem> GetCreatorByIdAsync(long id);
        Task<CreatorItem> CreateCreatorAsync(CreatorItem creator);
        Task UpdateCreatorAsync(CreatorItem existingCreator);
        Task DeleteCreatorAsync(long id);
        Task<bool> CreatorExistsAsync(long id);
    }
}
