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
using System.Collections.ObjectModel;

namespace TDDD49.ViewModel
{
    class MainViewModel : NotifyPropertyChangedBase
    {

        #region Private Fields
        private IConnectionService _ConnectionService;
        private MainModel Model;
        private UserControl _CurrentChatView;
        private string _SearchText = "";
        #endregion


        public MainViewModel(IConnectionService _ConnectionService, MainModel Model)
        {
            this._ConnectionService = _ConnectionService;
            this.Model = Model;
            Model.PropertyChanged += Model_PropertyChanged;

            CreateChatView(); //TODO: Only if currentchat
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
                    if (Model.CurrentConnection != null)
                    {
                        CreateChatView();
                    }
                    else
                        CurrentChatView = null;
                    break;
                case "Connected":
                    break;
                default:
                    OnPropertyChanged(e.PropertyName);
                    break;
            }
        }

        private void CreateChatView()
        {
            ChatView cv = new ChatView();
            cv.DataContext = new ChatViewModel(_ConnectionService.Disconnect, _ConnectionService.Send, param => Model.Connected && Model.ConnectedGuid == Model.CurrentConnection.id, Model.CurrentConnection);
            CurrentChatView = cv;
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

        public string SearchText
        {
            get
            {
                return _SearchText;
            }
            set
            {
                _SearchText = value;
                OnPropertyChanged("Connections");
            }
        }

        public UserControl CurrentChatView
        {
            get
            {
                return _CurrentChatView;
            }
            set
            {
                _CurrentChatView = value;
                OnPropertyChanged("CurrentChatView");
            }
        }
        public IEnumerable<ConnectionItemViewModel> Connections
        {
            get
            {
                return Model.Connections.Select((cm, index) => new ConnectionItemViewModel(cm, ()=>OnConnectionSelected(index))).Where((x) => x.Username.Contains(SearchText));
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
                return new RelayCommand(parameter => Actions.OpenDialog(typeof(ConnectDialog), new ConnectDialogViewModel(_ConnectionService.Connect)), parameter => !Model.Connected);
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

        #region Methods
        private void OnConnectionSelected(int i)
        {
            Model.CurrentConnection = Model.Connections[i];
        }

        #endregion
    }
}
