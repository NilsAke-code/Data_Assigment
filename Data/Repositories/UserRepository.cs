
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class UserRepository(DataContext context) : IRepository<UserEntity>
{
    private readonly DataContext _context = context;

    public async Task AddAsync(UserEntity entity)
    {
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserEntity>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    // Denna kod är genererad av CHATGPT 4.0 - Chat gpt gav som förslag att lägga till en metod för att söka efter användare via e-post.
    public async Task<UserEntity?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<UserEntity?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task UpdateAsync(UserEntity entity)
    {
        _context.Users.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Users.FindAsync(id);
        if (entity != null)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
