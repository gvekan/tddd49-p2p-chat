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
        private string _Username;
        private string _IPAddrPort;

        // TODO: Use lock
        public ObservableCollection<MessageModel> Messages = new ObservableCollection<MessageModel>();

        public ConnectionModel(string Username, string IPAddrPort, ObservableCollection<MessageModel> Messages = null)
        {
            this.Username = Username;
            this.IPAddrPort = IPAddrPort;

            if (Messages != null)
            {
                Messages.Count();
                this.Messages = Messages;
            }
            this.Messages.CollectionChanged += Messages_CollectionChanged;
        }

        private void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Messages");
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
    }
}
