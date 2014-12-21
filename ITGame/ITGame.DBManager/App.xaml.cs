using ITGame.DBConnector;
using ITGame.DBManager.Data;
using ITGame.DBManager.ViewModels;
using ITGame.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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

            var viewmodel = new MainViewModel(new DBRepository(), new EntityViewModelBuilder());

            var window = new MainWindow { DataContext = viewmodel };

            window.Show();
        }
    }
}
