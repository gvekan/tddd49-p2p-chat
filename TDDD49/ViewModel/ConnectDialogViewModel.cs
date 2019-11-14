using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TDDD49.Commands;
using TDDD49.Helpers;

namespace TDDD49.ViewModel
{
    class ConnectDialogViewModel : NotifyPropertyChangedBase
    {
        private string _IPAddr = "";
        private string _Port = "";
        private Action<string, string> _ConnectAction;

        public ConnectDialogViewModel(Action<string, string> ConnectAction)
        {
            _ConnectAction = ConnectAction;
        }

        public string IPAddr
        {
            get { return _IPAddr; }
            set
            {
                _IPAddr = value;
                OnPropertyChanged(IPAddr);
            }
        }

        public string Port
        {
            get { return _Port; }
            set
            {
                _Port = value;
                OnPropertyChanged(Port);
            }
        }

        public void Connect(object param)
        {
            if(Port.Length > 0 && IPAddr.Length > 0)
            {
                MessageBox.Show("Attempting connect " + Port + " " + IPAddr);
                try
                {
                    _ConnectAction(_IPAddr, _Port);
                } catch(InvalidIPException)
                {
                    MessageBox.Show("Could not connect to the specified IP and PORT. Please try again with different settings");
                    return;
                }
                
                
            }
            else
            {
                MessageBox.Show("Please enter a valid IP Address");
                return;
            }
            Actions.CloseDialog(param);
        }
        #region Commands

        public ICommand ConnectCommand
        {
            get
            {
                return new RelayCommand(param => this.Connect(param));
            }
        }
        #endregion
    }
}
