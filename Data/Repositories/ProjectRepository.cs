using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProjectRepository(DataContext context) : IRepository<ProjectEntity>
{
    private readonly DataContext _context = context;

    public async Task AddAsync(ProjectEntity entity)
    {
        await _context.Projects.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProjectEntity>> GetAllAsync()
    {
        return await _context.Projects
        .Include(p => p.Customer)
        .Include(p => p.Service)
        .Include(p => p.User)
        .Include(p => p.Status)
        .ToListAsync();
    }

    public async Task<ProjectEntity?> GetByIdAsync(int id)
    {
        return await _context.Projects
       .Include(p => p.Customer)
       .Include(p => p.Service)
       .Include(p => p.User)
       .Include(p => p.Status)
       .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task UpdateAsync(ProjectEntity entity)
    {
        _context.Projects.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Projects.FindAsync(id);
        if (entity != null)
        {
            _context.Projects.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
