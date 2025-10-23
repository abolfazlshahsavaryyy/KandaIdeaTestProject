using AgriculturalLandManagement.Models;

public interface ICornerRepository
{
    Task<IEnumerable<Corner>> GetAllAsync();
    Task<Corner?> GetByIdAsync(int id);
    Task<bool> CreateAsync(Corner corner);
    Task<bool> UpdateAsync(int id, Corner corner);
    Task<bool> DeleteAsync(int id);
}
