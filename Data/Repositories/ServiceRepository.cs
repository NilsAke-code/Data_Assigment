using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ServiceRepository(DataContext context) : IRepository<ServiceEntity>
{
    private readonly DataContext _context = context;

    public async Task AddAsync(ServiceEntity entity)
    {
        await _context.Services.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ServiceEntity>> GetAllAsync()
    {
        return await _context.Services.ToListAsync();
    }

    public async Task<ServiceEntity?> GetByIdAsync(int id)
    {
        return await _context.Services.FindAsync(id);
    }

    public async Task UpdateAsync(ServiceEntity entity)
    {
        _context.Services.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Services.FindAsync(id);
        if (entity != null)
        {
            _context.Services.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
