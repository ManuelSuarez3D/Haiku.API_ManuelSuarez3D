using Haiku.API.Models;

namespace Haiku.API.Repositories
{
    public interface ICreatorRepository
    {
        Task<IEnumerable<CreatorItem>> GetCreatorsAsync(int page, int pageSize);
        Task<int> GetTotalCreatorsAsync();
        Task<CreatorItem> GetCreatorByIdAsync(long id);
        Task<CreatorItem> AddCreatorAsync(CreatorItem creator);
        Task UpdateCreatorAsync(CreatorItem creator);
        Task DeleteCreatorAsync(long id);
        Task<bool> CreatorExistsAsync(long id);
    }
}
