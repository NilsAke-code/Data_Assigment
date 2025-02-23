using Data.Entities;

namespace Data.Factories;

public static class ServiceFactory
{
    public static ServiceEntity Create(string serviceName, decimal price)
    {
        return new ServiceEntity
        {
            ServiceName = serviceName,
            Price = price
        };
    }
}
