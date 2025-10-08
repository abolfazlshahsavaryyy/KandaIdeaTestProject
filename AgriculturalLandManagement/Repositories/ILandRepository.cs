using AgriculturalLandManagement.Models;

namespace AgriculturalLandManagement.Repositories
{
    public interface ILandRepository
    {
        Task<IEnumerable<Land>> GetAllAsync();
        Task<Land?> GetByIdAsync(int id);
        Task<Land> AddAsync(Land land);
        Task<Land?> UpdateAsync(Land land);
        Task<bool> DeleteAsync(int id);
    }
}
