using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TDDD49.Commands;
using TDDD49.Exceptions;
using TDDD49.Helpers;
using TDDD49.Models;

namespace TDDD49.ViewModel
{
    class ChatViewModel : NotifyPropertyChangedBase
    {
        private Action DisconnectAction;
        private Action<MessageModel> SendAction;
        private Func<object, bool> CanExecute;
        private ConnectionModel Model;
        private string _TextMessage;


        public ChatViewModel(Action DisconnectAction, Action<MessageModel> SendAction, Func<object, bool> CanExecute, ConnectionModel Model)
        {
            this.Model = Model;
            this.DisconnectAction = DisconnectAction;
            this.CanExecute = CanExecute;
            this.SendAction = SendAction;
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

        public string TextMessage
        {
            get
            {
                return _TextMessage;
            }
            set
            {
                _TextMessage = value;
                OnPropertyChanged("TextMessage");
            }
        }

        public ICommand SendCommand
        {
            get
            {
                return new RelayCommand(SendTextMessage, param => !string.IsNullOrWhiteSpace(TextMessage) && CanExecute(param));
            }
        }

        private void SendTextMessage(object param)
        {
            MessageModel message = new TextMessageModel(TextMessage, true);
            message.StatusMessage = "Pending";
            Model.Messages.Add(message);
            TextMessage = "";
            try
            {
                SendAction(message);
            } catch (NoConnectionException e)
            {
                message.StatusMessage = "Not delivered";
            }

        }
    }
}
