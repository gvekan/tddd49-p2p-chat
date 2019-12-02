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

            MainModel.MainModelParams Params = new MainModel.MainModelParams();
            Params.Port = 6536;
            Params.Username = "Gabriel";
            Params.IP = "0.0.0.0";
            Params.Connections = new ObservableCollection<ConnectionModel>();
            for (int i = 0; i<10; i++)
            {
                Params.Connections.Add(new ConnectionModel("user" + i, "", new ObservableCollection<MessageModel>()));
            }
            Params.CurrentConnection = new ConnectionModel("No current connection", "", null);

            MainModel _MainModel = new MainModel(Params);


            ConnectionService _ConnectionService = new ConnectionService(_MainModel);

            MainWindow = new MainWindow();
            MainWindow.DataContext = new MainViewModel(_ConnectionService, _MainModel);

            this.Exit += _ConnectionService.OnExit;



            MainWindow.Show();
        }
    }
}
