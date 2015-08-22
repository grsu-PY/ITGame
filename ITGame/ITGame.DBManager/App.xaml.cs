using ITGame.DBManager.Data;
using ITGame.DBManager.ViewModels;
using ITGame.Infrastructure.Data;
using System.Windows;
using ITGame.DBConnector;
using ITGame.DBManager.Navigations;

namespace ITGame.DBManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IEntityRepository repositoryXML = new EntityRepository<EntityProjectorXml>();
            IEntityRepository repositoryDb = new DbRepository();

            IEntityViewModelBuilder viewModelBuilder = new EntityViewModelBuilder();
            INavigation navigation = new Navigation(viewModelBuilder);

            var viewmodel = new MainViewModel(navigation, repositoryXML);

            var window = new MainWindow { DataContext = viewmodel };

            window.Show();
        }
    }
}
