using AgriculturalLandManagement.Models;
using AgriculturalLandManagement.Repositories;
namespace AgriculturalLandManagement.Service;
public class LandService : ILandService
{
    private readonly ILandRepository _landRepository;
    private readonly ILogger<LandService> _logger;

    public LandService(ILandRepository landRepository, ILogger<LandService> logger)
    {
        _landRepository = landRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Land>> GetAllAsync()
    {
        return await _landRepository.GetAllAsync();
    }

    public async Task<Land?> GetByIdAsync(int id)
    {
        return await _landRepository.GetByIdAsync(id);
    }

    public async Task<Land> CreateAsync(Land land)
    {
        try
        {
            return await _landRepository.AddAsync(land);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating land: {ex.Message}");
            throw;
        }
    }

    public async Task<Land?> UpdateAsync(Land land)
    {
        return await _landRepository.UpdateAsync(land);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _landRepository.DeleteAsync(id);
    }
}
