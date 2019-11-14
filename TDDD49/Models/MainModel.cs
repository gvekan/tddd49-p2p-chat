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
                _CurrentConnection = value;
                OnPropertyChanged("CurrentConnection");
            }
        }

        #endregion
    }
}
