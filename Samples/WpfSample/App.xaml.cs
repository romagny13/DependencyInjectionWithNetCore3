using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Windows;
using WpfSample.Services;
using WpfSample.ViewModels;
using WpfSample.Views;

namespace WpfSample
{
    public partial class App : Application
    {
        private IHost host;

        public App()
        {
            host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddJsonFile("appsettings.local.json", true);
                })
                .ConfigureServices((context, services) =>
                {
                    ConfigureContainer();
                    RegisterServices(context.Configuration, services);
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddDebug();
                })
                .Build();
        }

        private void ConfigureContainer()
        {
            // Resolve view model with DependencyInjection Container
            ViewModelLocationProvider.SetViewModelFactory(type => host.Services.GetRequiredService(type));
        }

        private void RegisterServices(IConfiguration configuration, IServiceCollection services)
        {
            // map appsettings to model
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            // services
            services.AddSingleton<IService, ServiceA>();

            // view models
            services.AddSingleton<MainWindowViewModel>();

            // views
            services.AddSingleton<MainWindow>();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // create shell
            var mainWindow = host.Services.GetRequiredService<MainWindow>();

            // initialize shell
            MainWindow = mainWindow;

            // show shell
            mainWindow.Show();
        }
    }

    // Model for appsettings.json
    public class AppSettings
    {
        public string MySetting { get; set; }
    }
}
