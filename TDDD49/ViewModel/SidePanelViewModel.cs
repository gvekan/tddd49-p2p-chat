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

namespace TDDD49.ViewModel
{
    class SidePanelViewModel : ViewModelBase
    {
        #region Private Fields
        private String _Username;
        private String _UserIPAddr;
        private String _CurrentChatName;
        private String _CurrentChatIPAddr;

        // TODO: Add list of connections
        #endregion

        public SidePanelViewModel()
        {
            Username = "Dummy Name";
            UserIPAddr = "No connection";
            CurrentChatName = "My Friends Dummy Name";
            CurrentChatIPAddr = "No connection";
        }

        #region Public Properties

        public String Username
        {
            get
            {
                return _Username;
            }

            set
            {
                _Username = value;
                OnPropertyChanged("UserName");
            }
        }

        public String UserIPAddr
        {
            get
            {
                return _UserIPAddr;
            }

            set
            {
                _UserIPAddr = value;
                OnPropertyChanged("UserIpAdrr");
            }
        }

        public String CurrentChatIPAddr
        {
            get
            {
                return _CurrentChatIPAddr;
            }

            set
            {
                _CurrentChatIPAddr = value;
                OnPropertyChanged("CurrentChatIpAdrr");
            }
        }

        public String CurrentChatName
        {
            get
            {
                return _CurrentChatName;
            }

            set
            {
                _CurrentChatName = value;
                OnPropertyChanged("CurrentChatName");
            }
        }


        #endregion

        #region Commands

        public ICommand OpenConnectDialogCommand
        {
            get
            {
                // TODO: Add canExecute Func to detect internet connection
                return new RelayCommand(parameter => Actions.OpenDialog(typeof(ConnectDialog)));
            }
        }
        public ICommand OpenSettingsDialogCommand
        {
            get
            {
                // TODO: Add canExecute Func to detect internet connection
                return new RelayCommand(parameter => Actions.OpenDialog(typeof(SettingsDialog)));
            }
        }

        #endregion
    }
}
