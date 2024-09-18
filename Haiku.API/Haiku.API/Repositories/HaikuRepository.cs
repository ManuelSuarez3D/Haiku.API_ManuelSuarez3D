using Haiku.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Haiku.API.Repositories
{
    public class HaikuRepository : IHaikuRepository
    {
        private readonly HaikuAPIContext _context;
        private readonly ILogger<HaikuRepository> _logger;

        public HaikuRepository(HaikuAPIContext context, ILogger<HaikuRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<HaikuItem>> GetHaikusAsync(int page, int pageSize)
        {
            try
            {
                return await _context.HaikuItems
                    .OrderBy(c => c.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "An error occurred while fetching haikus from the database.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching haikus in Repository.");
                throw;
            }
        }

        public async Task<int> GetTotalHaikusAsync()
        {
            try
            {
                return await _context.HaikuItems.CountAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "An error occurred while counting haikus from the database.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while counting haikus in Repository.");
                throw;
            }
        }

        public async Task<HaikuItem> GetHaikuByIdAsync(long id)
        {
            try
            {
                return await _context.HaikuItems.FindAsync(id);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "An error occurred while fetching haiku from the database.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching haiku in Repository.");
                throw;
            }
        }

        public async Task<HaikuItem> CreateHaikuAsync(HaikuItem haiku)
        {
            try
            {
                var entity = await _context.HaikuItems.AddAsync(haiku);
                await _context.SaveChangesAsync();
                return entity.Entity;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "An error occurred while adding haiku to the database.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding haiku in Repository.");
                throw;
            }
        }

        public async Task UpdateHaikuAsync(HaikuItem haiku)
        {
            try
            {
                _context.HaikuItems.Update(haiku);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "An error occurred while updating haiku in the database.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating haiku in Repository.");
                throw;
            }
        }

        public async Task DeleteHaikuAsync(long id)
        {
            try
            {
                var haiku = await _context.HaikuItems.FindAsync(id);

                if (haiku != null)
                {
                    _context.HaikuItems.Remove(haiku);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "An error occurred while deleting haiku from the database.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting haiku in Repository.");
                throw;
            }
        }

        public async Task<bool> HaikuExistsAsync(long id)
        {
            try
            {
                return await _context.HaikuItems.AnyAsync(e => e.Id == id);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "An error occurred while checking existence of haiku in the database.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while checking existence of haiku in Repository.");
                throw;
            }
        }
    }
}
