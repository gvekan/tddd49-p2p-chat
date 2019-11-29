using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TDDD49.Commands;
using TDDD49.Helpers;
using TDDD49.Models;

namespace TDDD49.ViewModel
{
    class ConnectionItemViewModel : NotifyPropertyChangedBase
    {
        private ConnectionModel Model;
        private Action _OnClick;

        public ConnectionItemViewModel(ConnectionModel Model, Action OnClick)
        {
            this.Model = Model;
            Model.PropertyChanged += Model_PropertyChanged;
            _OnClick = OnClick;
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Username" || e.PropertyName == "IPAddrPort") 
            {
                OnPropertyChanged(e.PropertyName);
            }
        }

        public string Username
        {
            get
            {
                return Model.Username;
            }
        }

        public string IPAddrPort
        {
            get
            {
                return Model.IPAddrPort;
            }
        }

        public ICommand OnClick
        {
            get
            {
                return new RelayCommand(param => _OnClick());
            }
        }
    }
}
