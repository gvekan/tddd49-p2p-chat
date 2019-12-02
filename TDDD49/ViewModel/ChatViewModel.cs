using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TDDD49.Commands;
using TDDD49.Helpers;
using TDDD49.Models;

namespace TDDD49.ViewModel
{
    class ChatViewModel : NotifyPropertyChangedBase
    {
        private Action DisconnectAction;
        private Func<object, bool> CanExecute;
        private ConnectionModel Model;


        public ChatViewModel(Action DisconnectAction, Func<object, bool> CanExecute, ConnectionModel Model)
        {
            this.Model = Model;
            this.DisconnectAction = DisconnectAction;
            this.CanExecute = CanExecute;
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Messages")
                OnPropertyChanged("Messages");
        }

        public ICommand DisconnectCommand
        {
            get
            {
                return new RelayCommand(param => DisconnectAction(), CanExecute);
            }
        }

        public ObservableCollection<MessageModel> Messages
        {
            get
            {
                return Model.Messages;
            }
            set
            {

            }
        }
    }
}
