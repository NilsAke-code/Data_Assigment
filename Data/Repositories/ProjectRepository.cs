using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProjectRepository(DataContext context) : BaseRepository<ProjectEntity>(context)
{
    public override async Task<IEnumerable<ProjectEntity>> GetAllAsync()
    {
        return await _context.Projects
        .Include(p => p.Customer)
        .Include(p => p.Service)
        .Include(p => p.User)
        .Include(p => p.Status)
        .ToListAsync();
    }

    public override async Task<ProjectEntity?> GetByIdAsync(int id)
    {
        return await _context.Projects
       .Include(p => p.Customer)
       .Include(p => p.Service)
       .Include(p => p.User)
       .Include(p => p.Status)
       .FirstOrDefaultAsync(p => p.Id == id);
    }
}
