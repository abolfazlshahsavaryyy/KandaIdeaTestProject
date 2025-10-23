using Microsoft.EntityFrameworkCore;
using AgriculturalLandManagement.Data;
using AgriculturalLandManagement.Models;
using Microsoft.Extensions.Logging;

namespace AgriculturalLandManagement.Repositories
{
    public class LandRepository : ILandRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LandRepository> _logger;

        public LandRepository(AppDbContext context, ILogger<LandRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Land>> GetAllAsync()
        {
            try
            {
                return await _context.Lands
                    .Include(l => l.Corners)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving lands: {ex.Message}");
                return Enumerable.Empty<Land>();
            }
        }

        public async Task<Land?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Lands
                    .Include(l => l.Corners)
                    .FirstOrDefaultAsync(l => l.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving land with ID {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<Land> AddAsync(Land land)
        {
            try
            {
                _context.Lands.Add(land);
                await _context.SaveChangesAsync();
                return land;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding land: {ex.Message}");
                throw;
            }
        }

        public async Task<Land?> UpdateAsync(Land land)
        {
            try
            {
                var existing = await _context.Lands.FindAsync(land.Id);
                if (existing == null)
                {
                    _logger.LogWarning($"Land with ID {land.Id} not found.");
                    return null;
                }

                existing.OwnerName = land.OwnerName;
                existing.Production = land.Production;

                await _context.SaveChangesAsync();
                return existing;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating land with ID {land.Id}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var land = await _context.Lands.FindAsync(id);
                if (land == null)
                {
                    _logger.LogWarning($"Land with ID {id} not found.");
                    return false;
                }

                _context.Lands.Remove(land);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting land with ID {id}: {ex.Message}");
                return false;
            }
        }
    }
}
