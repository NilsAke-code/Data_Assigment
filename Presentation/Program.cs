using Data.Contexts;
using Data.Interfaces;
using Data.Repositories;
using Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Presentation;


class Program
{
    static async Task Main()
    {
        var serviceProvider = new ServiceCollection()
            .AddDbContext<DataContext>(options =>
             options.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Projects\\Data_Assigment\\Data\\Database\\local_database.mdf;Integrated Security=True;Connect Timeout=30"))
            .AddScoped(typeof(IRepository<>), typeof(BaseRepository<>))
            .AddScoped<ProjectService>()
            .AddScoped<CustomerService>()
            .AddScoped<ServiceManager>()
            .AddScoped<StatusTypeService>()
            .AddScoped<UserService>()
            .AddScoped<MenuDialog>()
            .BuildServiceProvider();
        try
        {
            var menuDialog = serviceProvider.GetRequiredService<MenuDialog>();
            await menuDialog.Start();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Ett fel uppstod: {ex.Message}");
            Console.ResetColor();
        }
    }
}
