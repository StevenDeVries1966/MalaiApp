using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WpfUI.Data;
using WpfUI.ViewModel;

namespace WpfUI
{
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection services = new();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddTransient<MainWindow>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<WorkedHoursViewModel>();
            services.AddTransient<JobsViewModel>();
            services.AddTransient<EmployeeViewModel>();
            services.AddTransient<ClientsViewModel>();
            services.AddTransient<IMalaiDataProvider, MalaiDataProvider>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow?.Show();
        }
    }
}
