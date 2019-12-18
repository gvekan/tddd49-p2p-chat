using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
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
            this.Model.LastMessage = DateTime.Now;
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

        public ICommand BuzzCommand
        {
            get
            {
                return new RelayCommand(Buzz, param => CanExecute(param));
            }
        }

        public ICommand SendCommand
        {
            get
            {
                return new RelayCommand(SendTextMessage, param => !string.IsNullOrWhiteSpace(TextMessage) && CanExecute(param));
            }
        }

        public ICommand ImageCommand
        {
            get
            {
                return new RelayCommand(SendImageMessage, param => CanExecute(param));
            }
        }

        private void Buzz(object param)
        {
            SendAction(new BuzzMessageModel(true));
        }

        private void SendImageMessage(object param)
        {
            string imagePath = null;
            BitmapImage img = null;
            Application.Current.Dispatcher.Invoke(() =>
               {
                   OpenFileDialog openFileDialog = new OpenFileDialog();
                   openFileDialog.Title = "Select image to send";
                   openFileDialog.Filter = "JPEG Files|*.jpeg";
                   Nullable<bool> res = openFileDialog.ShowDialog();
                   if (res == true)
                   {
                       imagePath = openFileDialog.FileName;
                   }
               });
            if(String.IsNullOrWhiteSpace(imagePath))
            {
                return;
            }
            img = new BitmapImage(new Uri(imagePath));
            
            MessageModel message = new ImageMessageModel(imagePath, true);
            message.StatusMessage = "Pending";

            Application.Current.Dispatcher.Invoke(() =>
            {
                Model.Messages.Add(message);
            });

            try
            {
                SendAction(message);
            }
            catch (NoConnectionException)
            {
                message.StatusMessage = "Not delivered";
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
            } catch (NoConnectionException)
            {
                message.StatusMessage = "Not delivered";
            }

        }
    }
}
