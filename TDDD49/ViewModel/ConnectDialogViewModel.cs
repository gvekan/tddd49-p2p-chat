using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TDDD49.ViewModel
{
    class ConnectDialogViewModel : ViewModelBase
    {
        private string _IPAddr;
        private string _Port;

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
        public void Connect()
        {
            
            MessageBox.Show("Not implemented yet");
        }
    }
}
