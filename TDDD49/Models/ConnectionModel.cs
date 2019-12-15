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
        public Guid id;

        // TODO: Use lock
        public MessageObservableCollection Messages = new MessageObservableCollection();

        public ConnectionModel(Guid id, string Username, string IPAddrPort, MessageObservableCollection Messages = null)
        {
            this.Username = Username;
            this.IPAddrPort = IPAddrPort;
            this.id = id;

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

        public class MessageObservableCollection : ObservableCollection<MessageModel>
        {
            public new void Add(MessageModel message)
            {
                
                base.Add(message);
                message.PropertyChanged += Message_PropertyChanged;
                
                
            }

            private void Message_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
            {
                OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
            }
        }
    }
}
