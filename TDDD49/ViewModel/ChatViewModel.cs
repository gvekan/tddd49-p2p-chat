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
    class ChatViewModel : NotifyPropertyChangedBase
    {
        private Action DisconnectAction;
        private Func<object, bool> CanExecute;

        public ChatViewModel(Action DisconnectAction, Func<object, bool> CanExecute)
        {
            this.DisconnectAction = DisconnectAction;
            this.CanExecute = CanExecute;
        }

        public ICommand DisconnectCommand
        {
            get
            {
                return new RelayCommand(param => DisconnectAction(), CanExecute);
            }
        }
    }
}
