using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TDDD49.Helpers;
using System.Collections.ObjectModel;

namespace TDDD49.Models
{
    class ConnectionModel : NotifyPropertyChangedBase
    {
        private Thread _Thread;

        private Socket _Socket;
        private string _Username;
        private string _IPAddrPort;

        public ObservableCollection<MessageModel> Messages;

        public ConnectionModel(string Username, string IPAddrPort, ObservableCollection<MessageModel> Messages, Thread t = null, Socket s = null)
        {
            this.Username = Username;
            this.IPAddrPort = IPAddrPort;
            this.Socket = s;

            this._Thread = t;
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

        public string IPAddrPort
        {
            get
            {
                return _IPAddrPort;
            }

            set
            {
                _IPAddrPort = value;
                OnPropertyChanged("IPAddrPort");
            }
        }

        public Socket Socket
        {
            get
            {
                return _Socket;
            }

            set
            {
                _Socket = value;
                OnPropertyChanged("Socket");
            }
        }
    }
}
