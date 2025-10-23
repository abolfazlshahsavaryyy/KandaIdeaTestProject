using AgriculturalLandManagement.Models;

public interface ICornerImageRepository
{
    Task<IEnumerable<CornerImage>> GetAllAsync();
    Task<CornerImage?> GetByIdAsync(int id);
    Task<bool> CreateAsync(CornerImage image);
    Task<bool> UpdateAsync(int id, byte[] imageData);
    Task<bool> DeleteAsync(int id);
    Task<bool> CreateAsync(CornerImage image, int landId, int cornerIndex);
}
