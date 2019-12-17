using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TDDD49.ViewModel;
using TDDD49.Services;
using TDDD49.Models;
using System.Collections.ObjectModel;

namespace TDDD49
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void AppStartup(object Sender, StartupEventArgs e)
        {

            DataService dataService = new DataService();


            MainModel _MainModel = dataService.Model;


            ConnectionService _ConnectionService = new ConnectionService(_MainModel, dataService);

            MainWindow = new MainWindow();
            MainWindow.DataContext = new MainViewModel(_ConnectionService, _MainModel);

            this.Exit += _ConnectionService.OnExit;



            MainWindow.Show();
        }
    }
}
