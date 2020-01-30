using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlQueryStressGUI.DbProviders;
using SqlQueryStressGUI.DbProviders.Views;
using SqlQueryStressGUI.Parameters;
using SqlQueryStressGUI.Parameters.Views;
using SqlQueryStressGUI.TestEnvironment;
using SqlQueryStressGUI.TestEnvironment.Views;
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

            var viewFactory = new ViewFactory();
            services.AddSingleton<IViewFactory>(viewFactory);
            ConfigureViews(viewFactory, services);

            ConfigureParameterSettingsViewModels(services);

            services.AddTransient<MainWindow>();

            services.AddTransient<ParameterViewModel>();
            services.AddTransient<ParameterSettingsViewModelBuilder>();
            services.AddTransient<ParameterViewModelBuilder>();

            services.AddTransient<QueryStressTestViewModel>();
            services.AddTransient<ConnectionManagerViewModel>();
            services.AddTransient<AddEditConnectionViewModel>();

            services.AddTransient<DbProviderFactory>();
            services.AddTransient<DbCommandProvider>();
            services.AddTransient<IConnectionProvider, ConnectionProvider>();
        }

        private void ConfigureViews(IViewFactory viewFactory, IServiceCollection services)
        {
            services.AddTransient<TestEnvironmentPage>();
            services.AddTransient<TestEnvironmentViewModel>();
            viewFactory.RegisterStartupPage<TestEnvironmentViewModel, TestEnvironmentPage>();

            services.AddTransient<ParameterManager>();
            viewFactory.Register<ParameterManagerViewModel, ParameterManager>();

            services.AddTransient<ParameterSettingsWindow>();
            viewFactory.Register<ParameterSettingsWindowViewModel, ParameterSettingsWindow>();

            services.AddTransient<RandomNumberView>();
            viewFactory.Register<RandomNumberParameterSettings, RandomNumberView>();

            services.AddTransient<RandomDateRangeView>();
            viewFactory.Register<RandomDateRangeParameterSettings, RandomDateRangeView>();

            services.AddTransient<MssqlParameterView>();
            viewFactory.Register<MssqlQueryParameterSettings, MssqlParameterView>();

            services.AddTransient<ConnectionWindow>();
            viewFactory.Register<AddEditConnectionViewModel, ConnectionWindow>();

            services.AddTransient<ConnectionManager>();
            viewFactory.Register<ConnectionManagerViewModel, ConnectionManager>();
        }

        private void ConfigureParameterSettingsViewModels(IServiceCollection services)
        {
            services.AddTransient<RandomDateRangeParameterSettings>();
            services.AddTransient<RandomNumberParameterSettings>();
            services.AddTransient<MssqlQueryParameterSettings>();
        }
    }
}
