using Data.Entities;
using Data.Interfaces;

namespace Data.Services;

public class StatusTypeService(IRepository<StatusTypeEntity> statusRepository)
{
    private readonly IRepository<StatusTypeEntity> _statusTypeRepository = statusRepository;

    public async Task<bool> CreateStatusAsync(StatusTypeEntity status)
    {
        if (string.IsNullOrEmpty(status.StatusName))
        {
            Console.WriteLine("Fel: Statusnamn får inte vara tomt.");
            return false;
        }
        await _statusTypeRepository.AddAsync(status);
        return true;
    }
    public async Task<IEnumerable<StatusTypeEntity>> GetStatusTypeEntitiesAsync()
    {
        return await _statusTypeRepository.GetAllAsync();
    }
    public async Task<StatusTypeEntity?> GetStatusByIdAsync(int id)
    {
        return await _statusTypeRepository.GetByIdAsync(id);
    }
}


