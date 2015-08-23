using ITGame.DBManager.Data;
using ITGame.DBManager.ViewModels;
using ITGame.Infrastructure.Data;
using System.Windows;
using ITGame.DBConnector;
using ITGame.DBManager.Navigations;
using ITGame.Infrastructure.Logging;
using Microsoft.Practices.Unity;

namespace ITGame.DBManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ILogger _logger;
        private IUnityContainer _unityContainer;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _unityContainer = ConfigureUnityContainer();
            _logger = _unityContainer.Resolve<ILogger>();
            _logger.LogStart();


            var viewmodel = _unityContainer.Resolve<MainViewModel>();

            var window = new MainWindow { DataContext = viewmodel };

            window.Show();
        }

        private static IUnityContainer ConfigureUnityContainer()
        {
            ILogger logger = new Logger();

            var unityContainer = new UnityContainer();
            unityContainer
                .RegisterInstance(logger)
                .RegisterType<IEntityRepository, EntityRepository<EntityProjectorXml>>()
                .RegisterType<INavigation, Navigation>(new ContainerControlledLifetimeManager());

            return unityContainer;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _logger.LogStop();
            base.OnExit(e);
        }
    }
}
