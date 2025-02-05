
using Data.Entities;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation;

public class MenuDialog(IServiceProvider serviceProvider)
{
    private readonly IRepository<ProjectEntity> _projectRepository = serviceProvider.GetRequiredService<IRepository<ProjectEntity>>();

    public async Task Start()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("==================================================");
            Console.WriteLine("              Projektadministration               ");
            Console.WriteLine("==================================================");
            Console.WriteLine("1. Visa lista över alla projekt");
            Console.WriteLine("2. Skapa ett nytt projekt");
            Console.WriteLine("3. Redigera/Uppdatera ett projekt");
            Console.WriteLine("4. Avsluta");
            Console.WriteLine("==================================================");
            Console.WriteLine("Välj ett alternativ: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ListProjects();
                    break;
                case "2":
                    await CreateProject();
                    break;
                case "3":
                    await EditProject();
                    break;
                case "4":
                    return;

                default:
                    Console.WriteLine("Ogiligt val, tryck på valfri tangent för att fortsätta...");
                    Console.ReadKey();
                    break;
            }
        }
    } 

    private async Task ListProjects()
    {
        Console.Clear();
        Console.WriteLine("==================================================");
        Console.WriteLine("               Lista över projekt                 ");
        Console.WriteLine("==================================================");

        var projects = await _projectRepository.GetAllAsync();
        foreach (var project in projects)
        {
            Console.WriteLine($"Projektnummer: {project.Id} | Namn: {project.Title} | Status: {project.Status.StatusName}");
        }
        Console.WriteLine("==================================================");
        Console.WriteLine("Tryck på valfri tangent för att gå tillbaka...");
        Console.ReadKey();
    }

    private async Task CreateProject()
    {
        Console.Clear();
        Console.WriteLine("==================================================");
        Console.WriteLine("               Skapa nytt projekt                 ");
        Console.WriteLine("==================================================");

        Console.Write("Ange projektnamn: ");
        var title = Console.ReadLine();

        Console.Write("Ange beskrivning: ");
        var description = Console.ReadLine();

        Console.Write("Ange Kundens ID: ");
        int customerId = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("Ange tjänstens ID: ");
        int serviceId = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("Ange användarens ID (projektledare): ");
        int userId = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("Ange statusens ID: ");
        int statusId = int.Parse(Console.ReadLine() ?? "0");

        // Validera startdatum - Denna kod är genererad av CHAT GPT 4.0, denna kod säkerställer att datumet som inmatas är giltigt.
        DateTime startDate;
        while (true)
        {
            Console.Write("Ange startdatum (YYYY-MM-DD): ");
            if (DateTime.TryParse(Console.ReadLine(), out startDate))
                break;
            Console.WriteLine("Ogiltigt startdatum. Försök igen.");
        }

        Console.Write("Ange slutdatum (YYYY-MM-DD): ");
        DateTime endDate = DateTime.Parse(Console.ReadLine() ?? "0001-01-01");

        var newProject = new ProjectEntity
        {
            Title = title,
            Description = description,
            CustomerId = customerId,
            ServiceId = serviceId,
            UserId = userId,
            StatusId = statusId,
            StartDate = startDate,
            EndDate = endDate
        };

        await _projectRepository.AddAsync(newProject);

        Console.WriteLine("\nProjektet har skapats! Tryck på valfri tangent för att gå tillbaka...");
        Console.ReadKey();
    }

    private async Task EditProject()
    {
        Console.Clear();
        Console.WriteLine("==================================================");
        Console.WriteLine("            Redigera/Uppdatera projekt            ");
        Console.WriteLine("==================================================");
        Console.Write("Ange projektnummer (ID) på projektet du vill redigera: ");
            int projectId = int.Parse(Console.ReadLine() ?? "0");
        
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
        {
            Console.WriteLine("Projektet hittades inte. Tryck på valfri tangent för att gå tillbaka");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"Redigerar projekt: {project.Title}");
        Console.WriteLine("Lämna fält tomma om du inte vill ändra dem.");

        Console.Write("Nytt projektnamn: ");
        var title = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(title))
            project.Title = title;

        Console.Write("Ny beskrivning: ");
        var description = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(description))
            project.Description = description;

        Console.Write("Ny status-ID: ");
        var statusId = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(statusId))
            project.StatusId = int.Parse(statusId);

        await _projectRepository.UpdateAsync(project);

        Console.WriteLine("\nProjektet har uppdaterats! Tryck på valfri tangent för att gå tillbaka...");
        Console.ReadKey();
    }
}


