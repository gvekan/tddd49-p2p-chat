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

namespace TDDD49
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void AppStartup(object Sender, StartupEventArgs e)
        { 

            MainModel.MainModelParams Params = new MainModel.MainModelParams();
            Params.Port = 6536;
            Params.Username = "Gabriel";
            Params.IP = "0.0.0.0";

            MainModel _MainModel = new MainModel(Params);


            ConnectionService _ConnectionService = new ConnectionService(_MainModel);

            MainWindow = new MainWindow();
            MainWindow.DataContext = new MainViewModel(_ConnectionService, _MainModel);



            MainWindow.Show();
        }
    }
}
