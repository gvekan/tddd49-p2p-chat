using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TDDD49.Commands;
using TDDD49.Views;
using System.Windows;
using System.Windows.Controls;
using TDDD49.Helpers;
using TDDD49.Services;
using TDDD49.Models;

namespace TDDD49.ViewModel
{
    class MainViewModel : NotifyPropertyChangedBase
    {

        #region Private Fields
        private IConnectionService _ConnectionService;
        private MainModel Model;
        #endregion

        // TODO: Remove these and use the model
        private String _CurrentChatName;
        private String _CurrentChatIPAddr;

        public MainViewModel(IConnectionService _ConnectionService, MainModel Model)
        {
            this._ConnectionService = _ConnectionService;
            this.Model = Model;
            Model.PropertyChanged += Model_PropertyChanged;


            CurrentChatName = "My Friends Dummy Name";
            CurrentChatIPAddr = "No connection";
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Port":
                    OnPropertyChanged("UserIPAddr");
                    break;
                case "CurrentConnection":
                    OnPropertyChanged("CurrentChatIPAddr");
                    OnPropertyChanged("CurrentChatName");
                    break;
                default:
                    OnPropertyChanged(e.PropertyName);
                    break;
            }
        }

        #region Public Properties

        public String Username
        {
            get
            {
                return Model.Username;
            }
        }

        public String UserIPAddr
        {
            get
            {
                return Model.IP + ":" + Model.Port.ToString();
            }
        }

        public String CurrentChatIPAddr
        {
            get
            {
                return Model.CurrentConnection.IPAddrPort;
            }

            set
            {
            }
        }

        public String CurrentChatName
        {
            get
            {
                return Model.CurrentConnection.Username;
            }

            set
            {
            }
        }


        #endregion

        #region Commands

        public ICommand OpenConnectDialogCommand
        {
            get
            {
                // TODO: Add canExecute Func to detect internet connection
                return new RelayCommand(parameter => Actions.OpenDialog(typeof(ConnectDialog), new ConnectDialogViewModel(_ConnectionService.Connect)));
            }
        }
        public ICommand OpenSettingsDialogCommand
        {
            get
            {
                // TODO: Add a SettingsDialogViewModel
                return new RelayCommand(parameter => Actions.OpenDialog(typeof(SettingsDialog), new SettingsDialogViewModel(Model)));
            }
        }

        #endregion
    }
}
