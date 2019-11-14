using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TDDD49.Helpers;
using TDDD49.ViewModel;
using TDDD49.Views;
using TDDD49.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace TDDD49.Services
{ 
    class ConnectionService : IConnectionService
    {
        #region Private Fields
        private IPEndPoint IP;
        private Thread ListenThread;
        private Socket ListenSocket;
        private MainModel Model;

        #endregion

        public ConnectionService(MainModel Model)
        {
            // TODO: Set message
            this.Model = Model;
            Model.PropertyChanged += Model_PropertyChanged;
            StartListen();
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Port":
                    StartListen();
                    break;
                case "Username":
                    // TODO: Send alert
                    break;
            }
        }

        public void StartListen()
        {
            if (ListenThread != null)
            {
                ListenThread.Abort();
                ListenSocket.Shutdown(SocketShutdown.Both);
            }

            IP = new IPEndPoint(IPAddress.Any, Model.Port);
            ListenThread = new Thread(new ThreadStart(() => Listen()));
            ListenThread.Start();
        }

        private void Listen()
        {
            Socket listenSocket = new Socket(AddressFamily.InterNetwork,
                                     SocketType.Stream,
                                     ProtocolType.Tcp);
            try
            {
                listenSocket.Bind(this.IP);
            } catch (Exception e)
            {
                Actions.HandleBugException(e, "Something went wrong with the connection. Try changing the port.");
                return;
            }

            listenSocket.Listen(10);
            while (true)
            {
                Socket conSocket = listenSocket.Accept();

                HandleConnection(conSocket);
            }
        }

        private void HandleConnection(Socket s)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Actions.OpenDialog(typeof(AcceptDeclineDialog),
                    new AcceptDeclineDialogViewModel(s.RemoteEndPoint.ToString(),
                        "Someone", parameter => InitConnection(s), parameter => s.Shutdown(SocketShutdown.Both)));
            });
        }

        public void InitConnection(Socket s)
        {
            Thread t = new Thread(new ThreadStart(() => HandleMessages(s)));
            Model.Connections.Add(new ConnectionModel("Dummy name", "0.0.0.0:4789", new ObservableCollection<MessageModel>()));
        }

        private void HandleMessages(Socket s)
        {
            while (s.Connected)
            {

            }
        }
       

        public bool Connect(string IP, string Port)
        {
            IPAddress IPAddr;
            try
            {
                IPAddress.TryParse(IP, out IPAddr);
            }
            catch (SocketException)
            {
                throw new InvalidIPException("Connection could not be made");
            }
            IPEndPoint ipe = new IPEndPoint(IPAddr, Convert.ToInt32(Port));
            Socket tempSocket =
                new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                tempSocket.BeginConnect(ipe);
            } catch(SocketException)
            {
                throw new InvalidIPException("Connection could not be made");
            }
        }

        private void StartConnection()
        {

        }
    }
}
