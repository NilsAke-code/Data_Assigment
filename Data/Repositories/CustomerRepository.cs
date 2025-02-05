

using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class CustomerRepository(DataContext context) : IRepository<CustomerEntity>
{
    private readonly DataContext _context = context;

    public async Task AddAsync(CustomerEntity entity)
    {
        await _context.Customers.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
 
    public async Task<IEnumerable<CustomerEntity>> GetAllAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<CustomerEntity?> GetByIdAsync(int id)
    {
        return await _context.Customers.FindAsync(id);
    }

    public async Task UpdateAsync(CustomerEntity entity)
    {
        _context.Customers.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Customers.FindAsync(id);
        if (entity != null)
        {
            _context.Customers.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
