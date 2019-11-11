using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49.ViewModel
{
    class AcceptDialogViewModel : ViewModelBase
    {

        private String _IP;
        private String _Username;
        private Action connectAction;
        private Action interuptAction;

        public AcceptDialogViewModel(String IP, String Username, Action connectAction)
        {
            this._IP = IP;
            this._Username = Username;
        }

        public String IP
        {
            get
            {
                return _IP;
            }
        }

        public String Username
        {
            get
            {
                return _Username;
            }
        }
    }
}
