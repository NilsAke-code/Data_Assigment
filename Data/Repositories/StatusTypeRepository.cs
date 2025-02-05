using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class StatusTypeRepository(DataContext context) : IRepository<StatusTypeEntity>
{
  private readonly DataContext _context = context;

    public async Task<IEnumerable<StatusTypeEntity>> GetAllAsync()
    {
        return await _context.StatusTypes.ToListAsync();
    }

    public async Task<StatusTypeEntity?> GetByIdAsync(int id)
    {
        return await _context.StatusTypes.FindAsync(id);
    }

    public Task UpdateAsync(StatusTypeEntity entity)
    {
        throw new NotImplementedException();
    }
    public Task AddAsync(StatusTypeEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}
