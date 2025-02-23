using Data.Entities;
using Data.Factories;
using Data.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation;

public class MenuDialog(IServiceProvider serviceProvider)
{
    private readonly ProjectService _projectService = serviceProvider.GetRequiredService<ProjectService>();
    private readonly CustomerService _customerService = serviceProvider.GetRequiredService<CustomerService>();
    private readonly ServiceManager _serviceManager = serviceProvider.GetRequiredService<ServiceManager>();
    private readonly StatusTypeService _statusTypeService = serviceProvider.GetRequiredService<StatusTypeService>();
    private readonly UserService _userService = serviceProvider.GetRequiredService<UserService>();
    private static int _listProjectsCallCount = 0;
    private List<ProjectEntity> _previousProjectList = new();

    // Efter jag la in min Transaction Management så började jag stöta på problem, vet inte varför mina menyalternativ dubblerades när jag gick ut och in ur menyalternativet i konsolen. 
    // Skapar ett projekt och går in i Visa Lista Över Projekt, då dyker Både titeln Skapa nytt Projekt upp med några av första stegen i min Visa Lista Över Projekt.
    // Går jag ut och in i Visa Lista Över Projekt så läggs det på en ny titel och alla redan existerande projekt.
    // En del är CHAT GPT för jag testat en del olika DEBUGS men även när jag ville få in metoderna att skriva ´avbryt´ eller ´´tillbaka´.
    public async Task Start()
    {
        Console.WriteLine("[DEBUG] Start() har anropats!");
        // Denna kod är genererad med CHAT GPT 4.0 - Huvudmenyn loopar tills användaren väljer att avsluta.
        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================");
            Console.WriteLine("              PROJEKTADMINISTRATION               ");
            Console.WriteLine("==================================================");
            Console.ResetColor();
            Console.WriteLine("1. Visa lista över alla projekt");
            Console.WriteLine("2. Skapa ett nytt projekt");
            Console.WriteLine("3. Redigera/Uppdatera ett projekt");
            Console.WriteLine("4. Avsluta");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Välj ett alternativ: ");
            Console.ResetColor();

            var choice = Console.ReadLine();

            
            switch (choice)
            {
                case "1":
                    Console.Clear();
                    await ListProjects();
                    break;
                case "2":
                    Console.Clear();
                    await CreateProject();
                    break;
                case "3":
                    Console.Clear();
                    await EditProject();
                    break;               
                case "4":
                    return;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ogiligt val, tryck på valfri tangent för att fortsätta...");
                    Console.ResetColor();
                    Console.ReadKey();
                    break;
            }
        }
    } 

    private async Task ListProjects()
    {
        Console.Clear();
        var projects = (await _projectService.GetProjectEntitiesAsync()).ToList();

        if (_previousProjectList.Count == projects.Count)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Inga nya projekt har tillkommit. Tryck på valfri tangent för att gå tillbaka.");
            Console.ResetColor();
            Console.ReadKey();
            return;
        }

        _previousProjectList = projects;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("==================================================");
        Console.WriteLine("               LISTA ÖVER PROJEKT                 ");
        Console.WriteLine("==================================================");
        Console.ResetColor();


        // Denna kod är genererad med CHAT GPT 4.0 - går över alla element/project och skriver ut information efter det.
        foreach (var project in projects)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================");
            Console.ResetColor();
            Console.WriteLine($"Projektnamn: {project.Title}");
            Console.WriteLine($"Kund: {project.Customer?.CustomerName ?? "Ingen kund angiven"}");
            Console.WriteLine($"Tjänst: {project.Service?.ServiceName ?? "Ingen tjänst angiven"} ({project.Service?.Price} kr/tim)");
            Console.WriteLine($"Projektledare: {project.User?.FirstName} {project.User?.LastName}");
            Console.WriteLine($"Startdatum: {project.StartDate:yyyy-MM-dd} | Slutdatum: {project.EndDate:yyyy-MM-dd}");
            Console.WriteLine($"Status: {project.Status?.StatusName ?? "Ingen status angiven"}");

        }
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("==================================================");
        Console.WriteLine("Tryck på valfri tangent för att gå tillbaka...");
        Console.ResetColor();
        Console.ReadKey();
    }

    private async Task CreateProject()
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("==================================================");
        Console.WriteLine("               SKAPA NYTT PROJEKT                 ");
        Console.WriteLine("==================================================");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("(Skriv 'Avbryt' för att återgå till menyn)");
        Console.WriteLine("(Skriv 'Tillbaka' för att gå till föregående steg)");      
        Console.ResetColor();

        int customerId = -1, serviceId = -1, userId = -1, statusId = -1;

        while (customerId == -1)  // Denna kod är genererad av CHAT GPT 4.0 - Varje loop anropar en metod för att hämta giltig inmatning från användaren.
        {
            customerId = await GetCustomerId();
            if (customerId == -2) return;
        }

        while (serviceId == -1)
        {
            serviceId = await GetServiceId();
            if (serviceId == -2) return;
            if (serviceId == -1)
            {
                customerId = -1;
                continue;
            }
        }

        while (userId == -1)
        {
            userId = await GetUserId();
            if (userId == -2) return;
            if (userId == -1)
            {
                serviceId = -1;
                continue;
            }
        }

        while (statusId == -1)
        {
            statusId = await GetStatusId();
            if (statusId == -2) return;
            if (statusId == -1)
            {
                userId = -1;
                continue;
            }
        }

        string? title = null;
        do
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Ange projektnamn: ");
            Console.ResetColor();
            var input = GetValidatedInput();

            if (input == "TILLBAKA")
            {
                statusId = -1;
                continue;
            }

            if (input == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Projektnamn får inte vara tomt. Försök igen.");
                Console.ResetColor();
                continue;
            }

            title = input;
        } while (title == null);

        string? description;
        do
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Ange beskrivning: ");
            Console.ResetColor();
            description = GetValidatedInput();
            if (description == "TILLBAKA") title = null;
            if (description == null) return;
        } while (description == "TILLBAKA");

        DateTime startDate;
        do
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            startDate = GetValidatedDate("Ange startdatum (YYYY-MM-DD): ");
            Console.ResetColor();
            if (startDate == DateTime.MinValue) return;
        } while (startDate == DateTime.MinValue);

        DateTime endDate;
        do
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            endDate = GetValidatedDate("Ange slutdatum (YYYY-MM-DD): ");
            Console.ResetColor();                       // Denna kod är genererad av CHAT GPT 4.0 - 
            if (endDate == DateTime.MinValue) return;   // Om användaren anger ett ogiltigt datum (DateTime.MinValue), avbryts metoden.
                                                        // Denna kod är genererad av CHAT GPT 4.0 - 
                                                        // Säkerställer att slutdatum inte är före eller samma som startdatumet.
            if (endDate > startDate) break;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Slutdatum måste vara efter startdatum. Försök igen.");
            Console.ResetColor();
        } while (true);

        var project = ProjectFactory.Create(title ?? "Okänt projekt", description ?? "Ingen beskrivning angiven", customerId, serviceId, userId, statusId, startDate, endDate);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Sparar projekt: {project.Title} - KundID: {project.CustomerId}, TjänstID: {project.ServiceId}, UserID: {project.UserId}, StatusID: {project.StatusId}");
        Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
        Console.ResetColor();
        Console.ReadKey();

        var success = await _projectService.CreateProjectWithTransactionAsync(project);
        if (success)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Projektet har skapats! Tryck på valfri tangent för att gå tillbaka...");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ett fel uppstod vid skapandet av projektet.");
            Console.ResetColor();
        }
        Console.ReadKey();
    }

    // Denna kod är genererad av CHAT GPT 4.0 -
    // Dessa metoder används för att hämta eller skapa en ny post för Status, Kund, Tjänst och Projektledare.
    // De anropar den metoden ChooseFromList för att hantera urval och möjliggöra skapande av nya poster.
    private async Task<int> GetStatusId()
    {
        return await ChooseFromList(
            "status",
            (await _statusTypeService.GetStatusTypeEntitiesAsync()).Select(s => (s.Id, s.StatusName)),
            async () =>
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Ange statusnamn: ");
                Console.ResetColor();
                var statusName = GetValidatedInput();
                if (statusName == null) return -2;  // Om användaren avbryter, returnerar -2 för att avbryta.

                var newStatus = StatusTypeFactory.Create(statusName);
                await _statusTypeService.CreateStatusAsync(newStatus);
                var statuses = await _statusTypeService.GetStatusTypeEntitiesAsync();
                return statuses.Last().Id;
            });
    }

    private async Task<int> GetCustomerId()
    {
        return await ChooseFromList(
            "kund",
            (await _customerService.GetCustomersAsync()).Select(c => (c.Id, c.CustomerName)),
            async () =>
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Ange kundens namn: ");
                Console.ResetColor();
                var customerName = GetValidatedInput();
                if (customerName == null) return -2;  // Om användaren avbryter, returnerar -2.

                var newCustomer = CustomerFactory.Create(customerName);
                await _customerService.CreateCustomerAsync(newCustomer);

                var customers = await _customerService.GetCustomersAsync();
                var createdCustomer = customers.LastOrDefault();

                if (createdCustomer == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ett fel uppstod när kunden skulle hämtas.");
                    Console.ResetColor();
                    return -2;  // Returnerar -2 för att signalera att något gick fel.
                }

                return createdCustomer.Id;
            });
    }

    private async Task<int> GetServiceId()
    {
        return await ChooseFromList(
            "tjänst",
            (await _serviceManager.GetAllServicesAsync()).Select(s => (s.Id, $"{s.ServiceName} ({s.Price} kr/tim)")),
            async () =>
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Ange tjänstens namn: ");
                Console.ResetColor();
                var serviceName = GetValidatedInput();
                if (serviceName == null) return -2;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Ange pris per timme: ");
                Console.ResetColor();
                decimal price = GetValidatedDecimal();
                if (price == -1) return -2;  // Validerar att priset är en giltig decimal.

                var newService = ServiceFactory.Create(serviceName, price);
                await _serviceManager.CreateServiceAsync(newService);
                var services = await _serviceManager.GetAllServicesAsync();
                return services.Last().Id;
            });
    }

    private async Task<int> GetUserId()
    {
        return await ChooseFromList(
            "projektledare",
            (await _userService.GetAllUserAsync()).Select(u => (u.Id, $"{u.FirstName} {u.LastName}")),
            async () =>
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Ange förnamn: ");
                Console.ResetColor();
                var firstName = GetValidatedInput();
                if (firstName == null) return -2;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Ange efternamn: ");
                Console.ResetColor();
                var lastName = GetValidatedInput();
                if (lastName == null) return -2;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Ange e-post: ");
                Console.ResetColor();
                var email = GetValidatedEmail();
                if (email == null) return -2;

                var newUser = UserFactory.Create(firstName, lastName, email);
                await _userService.CreateUserAsync(newUser);
                var users = await _userService.GetAllUserAsync();
                return users.Last().Id;
            });
    }
    // Denna kod är genererad av CHAT GPT 4.0 -
    // En generisk metod för att välja en befintlig post från en lista eller skapa en ny.
    // Detta används i flera delar av projektet där användaren behöver välja en kund, tjänst, användare eller status.

    private static async Task<int> ChooseFromList(
    string itemType,
    IEnumerable<(int Id, string Name)> items,
    Func<Task<int>> createNewItem)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Välj en {itemType} från listan eller skriv 'ny' för att skapa en ny:");
        Console.ResetColor();
        foreach (var (id, name) in items)
        {
            Console.WriteLine($"ID: {id} | {name}");
        }

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"Ange {itemType}-ID (eller skriv 'ny' för att skapa en ny {itemType}): ");
            Console.ResetColor();
            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltig inmatning. Försök igen.");
                Console.ResetColor();
                continue;
            }

            // Denna kod är genererad av CHAT GPT 4.0 - 
            // Om användaren skriver "avbryt", returnerar -2 som signal att avbryta processen.
            if (input.Equals("avbryt", StringComparison.OrdinalIgnoreCase)) return -2;

            // Denna kod är genererad av CHAT GPT 4.0 - 
            // Om användaren skriver "tillbaka", returnerar -1 för att signalera att vi ska gå tillbaka till föreående steg.
            if (input.Equals("tillbaka", StringComparison.OrdinalIgnoreCase)) return -1;

            // Denna kod är genererad av CHAT GPT 4.0 - 
            // Om användaren skriver "ny", anropa metoden för att skapa ett nytt objekt och returnera det nya objektets ID.
            if (input.Equals("ny", StringComparison.OrdinalIgnoreCase)) return await createNewItem();

            // Denna kod är genererad av CHAT GPT 4.0 - 
            // Om användaren skriver in ett numeriskt ID, kontrollera om det finns i listan.
            if (int.TryParse(input, out int selectedId) && items.Any(i => i.Id == selectedId))
            {
                return selectedId;  // Om ID:t finns i listan, returnera det valda ID:t.
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Ogiltigt ID. Ange ett existerande {itemType}-ID eller skriv 'ny'.");
            Console.ResetColor();
        }
    }

    private static string? GetValidatedInput()
    {
        while (true)
        {
            var input = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltig inmatning. Försök igen.");
                Console.ResetColor();
                continue;
            }

            // Denna kod är genererad av CHAT GPT 4.0
            // Om användaren skriver "avbryt" returneras null, vilket i sin tur avbryter hela processen.
            if (input.Equals("avbryt", StringComparison.OrdinalIgnoreCase)) return null;

            // Om användaren skriver "tillbaka" returneras "Tillbaka", vilket signalerar att programmet ska gå tillbaka ett steg.
            if (input.Equals("tillbaka", StringComparison.OrdinalIgnoreCase)) return "TILLBAKA";
            return input;
        }
    }

    private static string GetValidatedEmail()
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Ange e-post: ");
            Console.ResetColor();
            string email = (Console.ReadLine() ?? "").Trim();

            if (email.Equals("avbryt", StringComparison.OrdinalIgnoreCase)) return "avbryt";
            if (email.Equals("tillbaka", StringComparison.OrdinalIgnoreCase)) return "tillbaka";

            // Denna kod är genererad av CHAT GPT 4.0 - 
            // Kontrollerar om e-postadressen är giltig genom att se om den innehåller '@'.
            if (!string.IsNullOrWhiteSpace(email) && email.Contains('@'))
            {
                return email;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ogiltig e-post. Ange en giltig e-postadress:");
            Console.ResetColor();
        }
    }

    private static DateTime GetValidatedDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();

            // Denna kod är genererad av CHAT GPT 4.0 - 
            // Om användaren skriver "avbryt" returneras ett minimalt datumvärde för att signalera avbryt.
            if (input?.Equals("avbryt", StringComparison.OrdinalIgnoreCase) == true) return DateTime.MinValue;

            // Om användaren skriver "tillbaka" returneras ett minimalt datumvärde för att signalera att gå tillbaka.
            if (input?.Equals("tillbaka", StringComparison.OrdinalIgnoreCase) == true) return DateTime.MinValue;
            // Om inmatningen omvandlas till ett giltigt datum så returneras det.
            if (DateTime.TryParse(input, out DateTime date)) return date;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ogiltigt datum. Försök igen.");
            Console.ResetColor();
        }
    }

    private static decimal GetValidatedDecimal()
    {
        while (true)
        {
            var input = Console.ReadLine();

            // Denna kod är genererad av CHAT GPT 4.0 - 
            // Kontrollerar att användaren angav ett positivt decimaltal.
            if (decimal.TryParse(input, out decimal value) && value > 0) return value;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ogiltigt värde. Ange en siffra större än 0.");
            Console.ResetColor();
        }
    }


    private async Task EditProject()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("==================================================");
        Console.WriteLine("            REDIGERA/UPPDATERA PROJEKT            ");
        Console.WriteLine("==================================================");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("(Skriv 'Avbryt' när som helst för att återgå till menyn)");
        Console.ResetColor();

        var projects = (await _projectService.GetProjectEntitiesAsync()).ToList();

        if (projects.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Inga projekt hittades i databasen. Gå tillbaka och skapa ett projekt först.");
            Console.ResetColor();
            Console.ReadKey();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Tillgängliga projekt:");
        Console.ResetColor();

        foreach (var proj in projects)
        {
            Console.WriteLine($"ID: {proj.Id} | Namn: {proj.Title} | Status: {proj.Status?.StatusName ?? "Ingen status"}");
        }

        Console.WriteLine("==================================================");
        Console.Write("Ange projektnummer (ID) på projektet du vill redigera (eller skriv 'Avbryt' för att gå tillbaka): ");

        int projectId;
        while (true)
        {
            var input = Console.ReadLine()?.Trim();
            if (input?.Equals("avbryt", StringComparison.OrdinalIgnoreCase) == true) return;

            if (int.TryParse(input, out projectId) && projects.Any(p => p.Id == projectId))
                break;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ogiltigt ID. Ange ett existerande projektnummer eller skriv 'Avbryt' för att återgå:");
            Console.ResetColor();
        }

        var selectedProject = projects.First(p => p.Id == projectId);

        Console.WriteLine($"Redigerar projekt: {selectedProject.Title}");
        Console.WriteLine("Lämna fält tomma om du inte vill ändra dem.");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("Nytt projektnamn: ");
        Console.ResetColor();
        var title = Console.ReadLine()?.Trim();
        if (title?.Equals("avbryt", StringComparison.OrdinalIgnoreCase) == true) return; // Denna kod är genererad av CHAT GPT 4.0 - avbryt är genererat på flera ställen.
        if (!string.IsNullOrWhiteSpace(title)) selectedProject.Title = title;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("Ny beskrivning: ");
        Console.ResetColor();
        var description = Console.ReadLine()?.Trim();
        if (description?.Equals("avbryt", StringComparison.OrdinalIgnoreCase) == true) return;
        if (!string.IsNullOrWhiteSpace(description)) selectedProject.Description = description;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("Ny status-ID: ");
        Console.ResetColor();
        var statusId = Console.ReadLine()?.Trim();
        if (statusId?.Equals("avbryt", StringComparison.OrdinalIgnoreCase) == true) return;
        if (!string.IsNullOrWhiteSpace(statusId) && int.TryParse(statusId, out int newStatusId))
            selectedProject.StatusId = newStatusId;

        await _projectService.UpdateProjectAsync(selectedProject);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Projektet har uppdaterats! Tryck på valfri tangent för att gå tillbaka...");
        Console.ResetColor();
        Console.ReadKey();
    }
}


