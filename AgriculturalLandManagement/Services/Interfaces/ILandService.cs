using AgriculturalLandManagement.Models;

public interface ILandService
{
    Task<IEnumerable<Land>> GetAllAsync();
    Task<Land?> GetByIdAsync(int id);
    Task<Land> CreateAsync(Land land);
    Task<Land?> UpdateAsync(Land land);
    Task<bool> DeleteAsync(int id);
}
