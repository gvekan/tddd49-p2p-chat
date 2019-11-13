using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

using TDDD49.Commands;
using TDDD49.Models;
using TDDD49.Helpers;

namespace TDDD49.ViewModel
{
    class SettingsDialogViewModel : NotifyPropertyChangedBase
    {

        private UserModel Model;
        private string _Port;
        private string _Username;

        public SettingsDialogViewModel(UserModel Model)
        {
            this.Model = Model;
            Port = Model.Port.ToString();
            Username = Model.Username;
        }

        public string IP
        {
            get
            {
                return Model.IP;
            }

            set { } // Must be here else crash, dumb.
        }

        public string Port
        {
            get
            {
                return _Port;
            }

            set
            {
                _Port = value;
                OnPropertyChanged("Port");
            }
        }

        public string Username
        {
            get
            {
                return _Username;
            }

            set
            {
                _Username = value;
                OnPropertyChanged("Username");
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(parameter => Save(parameter), parameter => CanSave());
            }
        }

        private void Save(object parameter)
        {
            // TODO: Validate
            if (_Username != Model.Username)
            {
                Model.Username = _Username;
            }
            try
            {
                int NewPort = Int32.Parse(_Port);
                if (NewPort != Model.Port) {
                    Model.Port = NewPort;
                }
            }
            catch (Exception e)
            {
                Actions.HandleBugException(e, "Invalid Port value");
                return;
            }

            Actions.CloseDialog(parameter);
        }

        private bool CanSave()
        {
            // TODO: Validate
            return true;
        }
        
    }
}
