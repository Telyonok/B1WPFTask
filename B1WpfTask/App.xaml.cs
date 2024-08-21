using B1WPFTask.Data;
using B1WPFTask.Data.Readers;
using B1WPFTask.Data.Readers.Base;
using B1WPFTask.Data.Repositories;
using B1WPFTask.Data.Repositories.Interfaces;
using B1WPFTask.Models;
using B1WPFTask.ViewModels;
using B1WPFTask.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace B1WPFTask;
public partial class App
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<RandomRowDBContext>(options =>
                {
                    var connectionString = context.Configuration.GetConnectionString("RandomRowDB");
                    options.UseSqlServer(connectionString);
                });

                services.AddDbContext<BankDBContext>(options =>
                {
                    var connectionString = context.Configuration.GetConnectionString("BankDB");
                    options.UseSqlServer(connectionString);
                });

                services.AddScoped(typeof(IRepository<>), typeof(BankRepository<>));
                services.AddScoped<IRepository<RandomRow>, RandomRowRepository<RandomRow>>();

                services.AddScoped<IExcelReader<BankAccountClass>, ExcelReader>();

                services.AddSingleton<MainWindowViewModel>();

                services.AddSingleton((services) => new MainWindow()
                {
                    DataContext = services.GetRequiredService<MainWindowViewModel>()
                });
            })
            .Build();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _host.Start();

        MainWindow = _host.Services.GetRequiredService<MainWindow>();
        MainWindow.Show();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _host.StopAsync();
        _host.Dispose();

        base.OnExit(e);
    }
}