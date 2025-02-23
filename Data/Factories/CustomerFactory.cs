using Data.Entities;

namespace Data.Factories;

public static class CustomerFactory
{
    public static CustomerEntity Create(string customerName)
    {
        return new CustomerEntity
        {
            CustomerName = customerName
        };
    }
}
