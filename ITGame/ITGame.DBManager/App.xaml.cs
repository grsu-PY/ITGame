using ITGame.DBManager.Data;
using ITGame.DBManager.ViewModels;
using ITGame.Infrastructure.Data;
using System.Windows;

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

            var viewmodel = new MainViewModel(new EntityRepository<EntityProjectorXml>(), new EntityViewModelBuilder());

            var window = new MainWindow { DataContext = viewmodel };

            window.Show();
        }
    }
}
