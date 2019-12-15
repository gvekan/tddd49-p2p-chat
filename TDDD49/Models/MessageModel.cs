using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDDD49.Helpers;

namespace TDDD49.Models
{
    abstract class MessageModel : NotifyPropertyChangedBase
    {
        private string _StatusMessage = "";

        public MessageModel(bool _IsSender) 
        {
            IsSender = _IsSender;
        }

        public bool IsSender
        {
            get;
            set;
        }

        public string StatusMessage
        {
            get
            {
                return _StatusMessage;
            }
            set
            {
                _StatusMessage = value;
                OnPropertyChanged("StatusMessage");
            }
        }
    }


}
