using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDDD49.Helpers;
using System.Collections.ObjectModel;

namespace TDDD49.Models
{
    class MainModel : UserModel
    {
        public class MainModelParams : UserModelParams
        {
            public ConnectionModel CurrentConnection;
            public ObservableCollection<ConnectionModel> Connections;
        }

        private ConnectionModel _CurrentConnection;
        private bool _Connected;
        //private Guid _ConnectedGuid;

        public ObservableCollection<ConnectionModel> Connections;

        public MainModel(MainModelParams Params) : base(Params)
        {
            _CurrentConnection = Params.CurrentConnection;
            Connections = Params.Connections;
            Connections.CollectionChanged += Connections_CollectionChanged;
        }

        private void Connections_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Connections");
        }

        #region Properties

        public ConnectionModel CurrentConnection
        {
            get
            {
                return _CurrentConnection;
            }

            set
            {
                // TODO: Move current connection to connections list
                _CurrentConnection.PropertyChanged -= _CurrentConnection_PropertyChanged;
                _CurrentConnection = value;
                _CurrentConnection.PropertyChanged += _CurrentConnection_PropertyChanged;
                OnPropertyChanged("CurrentConnection");
            }
        }

        private void _CurrentConnection_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Username")
                OnPropertyChanged("CurrentConnection");
        }

        public bool Connected
        {
            get
            {
                return _Connected;
            }
            set
            {
                _Connected = value;
                OnPropertyChanged("Connected");
            }

        }

        public Guid ConnectedGuid
        {
            get;
            set;
        }

        #endregion
    }
}
