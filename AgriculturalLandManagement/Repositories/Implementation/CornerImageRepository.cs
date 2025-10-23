using AgriculturalLandManagement.Data;
using AgriculturalLandManagement.Models;
using Microsoft.EntityFrameworkCore;

public class CornerImageRepository : ICornerImageRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<CornerImageRepository> _logger;

    public CornerImageRepository(AppDbContext context, ILogger<CornerImageRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<CornerImage>> GetAllAsync()
    {
        try
        {
            return await _context.CornerImages
                .Include(i => i.Corner)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving images: {ex.Message}");
            return Enumerable.Empty<CornerImage>();
        }
    }

    public async Task<CornerImage?> GetByIdAsync(int id)
    {
        try
        {
            return await _context.CornerImages
                .Include(i => i.Corner)
                .FirstOrDefaultAsync(i => i.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving image with ID {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> CreateAsync(CornerImage image)
    {
        try
        {
            // ✅ Validate that the Corner exists
            var cornerExists = await _context.Corners.AnyAsync(c => c.Id == image.CornerId);
            if (!cornerExists)
            {
                _logger.LogWarning($"Corner with ID {image.CornerId} does not exist.");
                return false;
            }

            await _context.CornerImages.AddAsync(image);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating image: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateAsync(int id, byte[] imageData)
    {
        try
        {
            var existingImage = await _context.CornerImages.FindAsync(id);
            if (existingImage == null)
            {
                _logger.LogWarning($"Image with ID {id} not found.");
                return false;
            }

            existingImage.ImageData = imageData;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating image with ID {id}: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var existingImage = await _context.CornerImages.FindAsync(id);
            if (existingImage == null)
            {
                _logger.LogWarning($"Image with ID {id} not found.");
                return false;
            }

            _context.CornerImages.Remove(existingImage);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting image with ID {id}: {ex.Message}");
            return false;
        }
    }
    public async Task<bool> CreateAsync(CornerImage image, int landId, int cornerIndex)
    {
        try
        {
            // 1️⃣ Find the Corner using landId and cornerIndex
            var corner = await _context.Corners
                .FirstOrDefaultAsync(c => c.LandId == landId && c.Index == cornerIndex);
    
            if (corner == null)
            {
                _logger.LogWarning($"No corner found with LandId={landId} and Index={cornerIndex}");
                return false;
            }
    
            // 2️⃣ Set the foreign key
            image.CornerId = corner.Id;
    
            // 3️⃣ Add the image to the DbSet
            await _context.CornerImages.AddAsync(image);
    
            // 4️⃣ Save changes
            await _context.SaveChangesAsync();
    
            _logger.LogInformation($"Image added for CornerId={corner.Id} (LandId={landId}, Index={cornerIndex})");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating image for LandId={landId}, CornerIndex={cornerIndex}: {ex.Message}");
            return false;
        }
    }
    
}
