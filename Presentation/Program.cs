using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Presentation;
using System;


class Program
{
    static async Task Main()
    {
        var ServiceProvider = new ServiceCollection()
            .AddDbContext<DataContext>(options =>
             options.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Projects\\Data_Assigment\\Data\\Database\\local_database.mdf;Integrated Security=True;Connect Timeout=30"))
            .AddScoped<IRepository<CustomerEntity>, CustomerRepository>()
            .AddScoped<IRepository<ServiceEntity>, ServiceRepository>()
            .AddScoped<IRepository<UserEntity>, UserRepository>()
            .AddScoped<IRepository<ProjectEntity>, ProjectRepository>()
            .AddScoped<IRepository<StatusTypeEntity>, StatusTypeRepository>()
            .BuildServiceProvider();

        var MenuDialog = new MenuDialog(ServiceProvider);
        await MenuDialog.Start();
    }
}
