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

        // TODO: Use lock
        public ObservableCollection<ConnectionModel> Connections;

        public MainModel(MainModelParams Params) : base(Params)
        {
            _CurrentConnection = Params.CurrentConnection;
            Connections = Params.Connections;
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
                _CurrentConnection = value;
                OnPropertyChanged("CurrentConnection");
            }
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
            }

        }

        #endregion
    }
}
