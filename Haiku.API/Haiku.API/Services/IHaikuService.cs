using Haiku.API.Models;

namespace Haiku.API.Services
{
    public interface IHaikuService
    {
        Task<IEnumerable<HaikuItem>> GetHaikusAsync(int page, int pageSize);
        Task<int> GetTotalHaikusAsync();
        Task<HaikuItem> GetHaikuByIdAsync(long id);
        Task<HaikuItem> CreateHaikuAsync(HaikuItem haiku);
        Task UpdateHaikuAsync(HaikuItem haikuItem);
        Task DeleteHaikuAsync(long id);
        Task<bool> HaikuExistsAsync(long id);
    }
}
