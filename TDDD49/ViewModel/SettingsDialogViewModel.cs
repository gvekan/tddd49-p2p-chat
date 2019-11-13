using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
            _Port = Model.Port.ToString();
            _Username = Model.Username;
        }

        public string IP
        {
            get
            {
                return Model.IP;
            }
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
                return new RelayCommand(parameter => Save(), parameter => CanSave());
            }
        }

        private void Save()
        {
            
        }

        private bool CanSave()
        {
            return true;
        }
        
    }
}
