using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TDDD49.Helpers;

namespace TDDD49.Models
{
    class UserModel : NotifyPropertyChangedBase
    {
        public class UserModelParams
        {
            public int Port;
            public string Username;
            public string IP;
        }

        #region Private Fields
        private int _Port;
        private string _Username;
        private string _IP;
        #endregion

        public UserModel(UserModelParams Params)
        {
            IP = Params.IP;
            Port = Params.Port;
            Username = Params.Username;

        }

        #region Properties

        public string IP
        {
            get
            {
                return _IP;
            }

            set
            {
                _IP = value;
                OnPropertyChanged("IP");
            }
        }

        public int Port
        {
            get
            {
                return _Port;
            }

            set
            {
                _Port = value;
                OnPropertyChanged("Port");
            }
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

        #endregion
    }
}

