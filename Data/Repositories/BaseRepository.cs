using Data.Contexts;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories;

public class BaseRepository<T>(DataContext context) : IRepository<T> where T : class
{
    protected private readonly DataContext _context = context;
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        var data = await _context.Set<T>()
         .AsNoTracking()
         .ToListAsync();

        return data;
    }
    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    // Denna kod är genererad av CHAT GPT 4.0 - 
    // Denna metod används för att hämta alla poster av typen T från databasen,
    // samtidigt som relaterade entiteter inkluderas genom en lista av uttryck.
    // Det gör det möjligt att hämta data med navigationsrelationer (ex. inkludera en kunds projekt).
    public async Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes)
    {

        // Skapar en IQueryable för entiteten T i databaskontexten (_context)
        // IQueryable är en exekveringsfördröjd datakälla, vilket innebär att frågan inte skickas till databasen förrän vi faktiskt begär resultaten.
        IQueryable<T> query = _context.Set<T>();


        // Denna kod är genererad av CHAT GPT 4.0 - 
        // Itererar genom varje uttryck (lambda-funktion) i includes-parametern.
        // Exempel: Om vi skickar in "p => p.Customer", så kommer den att inkludera relaterad kunddata för projekten.
        foreach (var include in includes)
        {
            query = query.Include(include);  // Använder Entity Frameworks Include() för att ladda relaterad data
        }

        // Omvandlar frågan till en lista och exekverar den asynkront.
        // ToListAsync() skickar SQL-frågan till databasen och returnerar resultaten som en lista.
        return await query.ToListAsync();
    }
}
