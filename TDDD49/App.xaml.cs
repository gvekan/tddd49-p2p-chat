using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TDDD49
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ConnectionService _ConnectionService;
        void AppStartup(object Sender, StartupEventArgs e)
        {


            _ConnectionService = new ConnectionService();

            MainWindow = new MainWindow();
            MainWindow.Show();
        }
    }
}
