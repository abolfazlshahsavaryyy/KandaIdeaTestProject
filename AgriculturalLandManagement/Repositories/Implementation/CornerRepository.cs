using AgriculturalLandManagement.Data;
using AgriculturalLandManagement.Models;
using Microsoft.EntityFrameworkCore;

public class CornerRepository : ICornerRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<CornerRepository> _logger;

    public CornerRepository(AppDbContext context, ILogger<CornerRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> CreateAsync(Corner corner)
    {
        try
        {
            // ✅ Validate that the Land exists
            var landExists = await _context.Lands.AnyAsync(l => l.Id == corner.LandId);
            if (!landExists)
            {
                _logger.LogWarning($"Land with ID {corner.LandId} does not exist.");
                return false;
            }

            await _context.Corners.AddAsync(corner);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Db Connection Error with message: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var existingCorner = await _context.Corners.FindAsync(id);
            if (existingCorner == null)
            {
                _logger.LogWarning($"Corner with ID {id} not found.");
                return false;
            }

            _context.Corners.Remove(existingCorner);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Db Connection Error: {ex.Message}");
            return false;
        }
    }

    public async Task<IEnumerable<Corner>> GetAllAsync()
    {
        try
        {
            return await _context.Corners
                .Include(c => c.Land)
                .Include(c => c.Image)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving corners: {ex.Message}");
            return Enumerable.Empty<Corner>();
        }
    }

    public async Task<Corner?> GetByIdAsync(int id)
    {
        try
        {
            return await _context.Corners
                .Include(c => c.Land)
                .Include(c => c.Image)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving corner with ID {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> UpdateAsync(int id, Corner corner)
    {
        try
        {
            var existingCorner = await _context.Corners.FindAsync(id);
            if (existingCorner == null)
            {
                _logger.LogWarning($"Corner with ID {id} not found.");
                return false;
            }

            existingCorner.Latitude = corner.Latitude;
            existingCorner.Longitude = corner.Longitude;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating corner with ID {id}: {ex.Message}");
            return false;
        }
    }
}
