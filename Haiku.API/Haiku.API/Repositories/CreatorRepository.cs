using Haiku.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Haiku.API.Repositories
{
    public class CreatorRepository : ICreatorRepository
    {
        private readonly HaikuAPIContext _context;
        private readonly ILogger<CreatorRepository> _logger;

        public CreatorRepository(HaikuAPIContext context, ILogger<CreatorRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<CreatorItem>> GetCreatorsAsync(int page, int pageSize)
        {
            try
            {
                return await _context.Creators
                    .OrderBy(c => c.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "An error occurred while fetching creators from the database.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching creators in Repository.");
                throw;
            }
        }

        public async Task<int> GetTotalCreatorsAsync()
        {
            try
            {
                return await _context.Creators.CountAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "An error occurred while counting creators from the database.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while counting creators in Repository.");
                throw;
            }
        }

        public async Task<CreatorItem> GetCreatorByIdAsync(long id)
        {
            try
            {
                return await _context.Creators.FindAsync(id);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "An error occurred while fetching creator from the database.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching creator in Repository.");
                throw;
            }
        }

        public async Task<CreatorItem> AddCreatorAsync(CreatorItem creator)
        {
            try
            {
                var entity = await _context.Creators.AddAsync(creator);
                await _context.SaveChangesAsync();
                return entity.Entity;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "An error occurred while adding creator to the database.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding creator in Repository.");
                throw;
            }
        }

        public async Task UpdateCreatorAsync(CreatorItem creator)
        {
            try
            {
                _context.Creators.Update(creator);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "An error occurred while updating creator in the database.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating creator in Repository.");
                throw;
            }
        }
        public async Task DeleteCreatorAsync(long id)
        {
            try
            {
                var haikusToUpdate = await _context.HaikuItems
                    .Where(h => h.CreatorId == id)
                    .ToListAsync();

                foreach (var haiku in haikusToUpdate)
                {
                    haiku.CreatorId = 1;
                }

                var creator = await _context.Creators.FindAsync(id);
                if (creator != null)
                {
                    _context.Creators.Remove(creator);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "An error occurred while deleting creator from the database.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting creator in Repository.");
                throw;
            }
        }

        public async Task<bool> CreatorExistsAsync(long id)
        {
            try
            {
                return await _context.Creators.AnyAsync(e => e.Id == id);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "An error occurred while checking existence of creator in the database.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while checking existence of creator in Repository.");
                throw;
            }
        }
    }
}
