using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Services;

public class ProjectService(IRepository<ProjectEntity> projectRepository, DataContext context)
{
    private readonly IRepository<ProjectEntity> _projectRepository = projectRepository;
    private readonly DataContext _context = context;
    public async Task<bool> CreateProjectAsync(ProjectEntity project)
    {
        if (project.StartDate >= project.EndDate)
        {
            Console.WriteLine("Fel: Startdatum måste vara före slutdatum. ");
            return false;
        }
        await _projectRepository.AddAsync(project);
        await _projectRepository.SaveChangesAsync();
        return true;
    }


    // Denna kod är genererad av CHAT GPT 4.0 - 
    // Hämtar alla projekt från databasen och inkluderar relaterade entiteter (kund, tjänst, användare och status).
    public async Task<IEnumerable<ProjectEntity>> GetProjectEntitiesAsync()
    {
        var projects = await _projectRepository
            .GetAllWithIncludesAsync(
                p => p.Customer,
                p => p.Service,
                p => p.User,
                p => p.Status
            );

        Console.WriteLine($"[DEBUG] Nuvarande projekt antecknade: {projects.Count()}");
        return projects.ToList();
    }

    public async Task<ProjectEntity?> GetProjectByIdAsync (int id)
    {
        return await _projectRepository.GetByIdAsync(id);
    }

    // Denna kod är genererad av CHAT GPT 4.0 - 
    // Uppdaterar ett existerande projekt i databasen, först kontrollerar programmet om projektet finns i databasen, returnerar false om projektet inte finns. 
    // Om det existerar så uppdateras projektet och sparas sedan nya värdena i databasen,
    public async Task<bool> UpdateProjectAsync(ProjectEntity project)
    {
        var existingProject = await _projectRepository.GetByIdAsync (project.Id);
        if (existingProject == null)
        {
            Console.WriteLine("Fel: Projektet hittades inte.");
            return false;
        }

        existingProject.Title = project.Title;
        existingProject.Description = project.Description;
        existingProject.StartDate = project.StartDate;
        existingProject.EndDate = project.EndDate;
        existingProject.StatusId = project.StatusId;

        await _projectRepository.UpdateAsync(existingProject);
        return true;
    }

    // Denna kod är genererad av CHAT GPT 4.0 - 
    // Skapar ett projekt med transaktionshantering för att säkerställa att databasen inte hamnar i ett inkonsistent tillstånd.
    public async Task<bool> CreateProjectWithTransactionAsync(ProjectEntity project)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[DEBUG] Startar transaktionshantering...");
        Console.ResetColor();

        using var transaction = await _context.Database.BeginTransactionAsync(); // Startar en ny databastransaktion.

        try
        {
            await _projectRepository.AddAsync(project);// Lägger till projektet i databasen.
            await _context.SaveChangesAsync();// Sparar ändringarna.

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[DEBUG] Projekt sparat i databasen.");
            Console.ResetColor();

            await transaction.CommitAsync();// Commit för att bekräfta transaktionen.

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[DEBUG] Transaktion commitad.");
            Console.ResetColor();

            return true; // Returnerar true om transaktionen lyckades.
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();// Om ett fel uppstår rullas transaktionen tillbaka.
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Fel vid skapandet av projekt; {ex.Message}");
            Console.ResetColor();
            return false; // Returnerar false om något gick fel.
        }
    }
}
