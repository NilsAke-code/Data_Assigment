using Data.Entities;
using Data.Interfaces;

namespace Data.Services;

public class CustomerService(IRepository<CustomerEntity> customerRepository)
{
    private readonly IRepository<CustomerEntity> _customerRepository = customerRepository;

    public async Task<bool> CreateCustomerAsync(CustomerEntity customer)
    {
        if (customer == null || string.IsNullOrWhiteSpace(customer.CustomerName))
            return false;

        await _customerRepository.AddAsync(customer);
        return await _customerRepository.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<CustomerEntity>> GetCustomersAsync()
    {
        return await _customerRepository.GetAllAsync();
    }

    public async Task<CustomerEntity?> GetCustomerByIdAsync(int id)
    {
        return await _customerRepository.GetByIdAsync(id);
    }
}
