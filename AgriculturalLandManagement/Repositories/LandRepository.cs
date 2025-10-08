using Microsoft.EntityFrameworkCore;
using AgriculturalLandManagement.Data;
using AgriculturalLandManagement.Models;

namespace AgriculturalLandManagement.Repositories
{
    public class LandRepository : ILandRepository
    {
        private readonly AppDbContext _context;

        public LandRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Land>> GetAllAsync()
        {
            return await _context.Lands.ToListAsync();
        }

        public async Task<Land?> GetByIdAsync(int id)
        {
            return await _context.Lands.FindAsync(id);
        }

        public async Task<Land> AddAsync(Land land)
        {
            land.Area = Math.Abs(land.X1 - land.X2) * Math.Abs(land.Y1 - land.Y2);

            _context.Lands.Add(land);
            await _context.SaveChangesAsync();
            return land;
        }


        public async Task<Land?> UpdateAsync(Land land)
        {
            var existing = await _context.Lands.FindAsync(land.Id);
            if (existing == null)
                return null;
        
            // Update properties that can change
            existing.OwnerName = land.OwnerName;
            existing.Production = land.Production;
            existing.X1 = land.X1;
            existing.Y1 = land.Y1;
            existing.X2 = land.X2;
            existing.Y2 = land.Y2;
        
            // Recalculate area
            existing.Area = Math.Abs(land.X1 - land.X2) * Math.Abs(land.Y1 - land.Y2);
        
            await _context.SaveChangesAsync();
            return existing;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var land = await _context.Lands.FindAsync(id);
            if (land == null)
                return false;

            _context.Lands.Remove(land);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
