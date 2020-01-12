using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlQueryStressGUI.ViewModels;
using SqlQueryStressGUI.Views;
using System;
using System.Windows;

namespace SqlQueryStressGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            Configuration = new ConfigurationBuilder().Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
            DiContainer.Initialize(ServiceProvider);

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConnectionProvider, ConnectionProvider>();

            services.AddTransient<MainWindow>();
            services.AddTransient<QueryStressTestPage>();
            services.AddTransient<ConnectionManager>();
            services.AddTransient<ConnectionWindowFactory>();

            services.AddTransient<QueryStressTestViewModel>();
            services.AddTransient<ConnectionManagerViewModel>();
            services.AddTransient<AddEditConnectionViewModel>();

            services.AddTransient<DbProviderFactory>();
            services.AddTransient<DbCommandProvider>();
        }
    }
}
