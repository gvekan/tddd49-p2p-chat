using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TDDD49.Commands;
using TDDD49.Helpers;

namespace TDDD49.ViewModel
{
    class AcceptDeclineDialogViewModel : NotifyPropertyChangedBase
    {

        private String _Message;
        private Action<object> AcceptAction;
        private Action<object> CancelAction;

        public AcceptDeclineDialogViewModel(String IP, String Username, Action<object> AcceptAction, Action<object> CancelAction)
        {
            _Message = Username + " (" + IP + ") trying to connect to you.";
            this.AcceptAction = AcceptAction;
            this.CancelAction = CancelAction;
        }

        public String Message
        {
            get
            {
                return _Message;
            }
        }

        public ICommand AcceptCommand
        {
            get
            {
                return new RelayCommand(AcceptAction);
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommand(CancelAction);
            }
        }
    }
}
