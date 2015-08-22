using ITGame.DBManager.Data;
using ITGame.DBManager.ViewModels;
using ITGame.Infrastructure.Data;
using System.Windows;
using ITGame.DBConnector;
using ITGame.DBManager.Navigations;
using ITGame.Infrastructure.Logging;

namespace ITGame.DBManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ILogger _logger;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _logger = new Logger();
            _logger.LogStart();

            IEntityRepository repositoryXML = new EntityRepository<EntityProjectorXml>();
            IEntityRepository repositoryDb = new DbRepository();

            IEntityViewModelBuilder viewModelBuilder = new EntityViewModelBuilder();
            INavigation navigation = new Navigation(viewModelBuilder);

            var viewmodel = new MainViewModel(navigation, repositoryXML);

            var window = new MainWindow { DataContext = viewmodel };

            window.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _logger.LogStop();
            base.OnExit(e);
        }
    }
}
