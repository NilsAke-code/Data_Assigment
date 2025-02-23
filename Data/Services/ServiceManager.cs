using Data.Entities;
using Data.Interfaces;

namespace Data.Services;

public class ServiceManager(IRepository<ServiceEntity> serviceRepository)
{
    private readonly IRepository<ServiceEntity> _serviceRepository = serviceRepository;
    public async Task<bool> CreateServiceAsync(string serviceName, decimal price)
    {
        if (string.IsNullOrWhiteSpace(serviceName) || price <= 0)
        {
            Console.WriteLine("Fel: Tjänstens namn får inte vara tomt och priset måste vara större än 0.");
            return false;
        }

        var newService = new ServiceEntity { ServiceName = serviceName, Price = price };
        await _serviceRepository.AddAsync(newService);
        return true;
    }

    public async Task<bool> CreateServiceAsync(ServiceEntity service)
    {
        if (service == null || string.IsNullOrWhiteSpace(service.ServiceName) || service.Price <= 0)
            return false;

        await _serviceRepository.AddAsync(service);
        int saved = await _serviceRepository.SaveChangesAsync();

        return saved > 0;
    }

    public async Task<IEnumerable<ServiceEntity>> GetAllServicesAsync()
    {
        return await _serviceRepository.GetAllAsync();
    }

    public async Task<ServiceEntity?> GetServiceByIdAsync(int id)
    {
        return await _serviceRepository.GetByIdAsync(id);
    }
}
