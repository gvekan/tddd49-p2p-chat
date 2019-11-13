using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TDDD49.Helpers;
using TDDD49.Models;

namespace TDDD49.ViewModel
{
    class ConnectionItemViewModel : NotifyPropertyChangedBase
    {
        private ConnectionModel Model;

        public ConnectionItemViewModel(ConnectionModel Model)
        {
            this.Model = Model;
            Model.PropertyChanged += Model_PropertyChanged;
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
    }
}
