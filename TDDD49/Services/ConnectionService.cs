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
using TDDD49.Messages;
using System.IO;
using Newtonsoft.Json;

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
            // Read info here
            String buffer = "";
            byte[] bytes = new byte[1024];
            do
            {
                s.Receive(bytes);
                buffer += Encoding.UTF8.GetString(bytes).TrimEnd('\0');
                bytes = new byte[256];
            } while (buffer.Substring(buffer.Length - 5) != "<EOM>");
            buffer = buffer.Remove(buffer.Length-5);
            RequestConnectMessage msg = (RequestConnectMessage)JsonConvert.DeserializeObject(buffer, typeof(RequestConnectMessage));
            string ip = s.RemoteEndPoint.ToString(); // TODO: change port to RequestConnectMessage.Port
            ConnectionModel cm = new ConnectionModel(ip,
                       msg.Sender, null);
            Application.Current.Dispatcher.Invoke(() =>
            {
                Actions.OpenDialog(typeof(AcceptDeclineDialog),
                    new AcceptDeclineDialogViewModel(ip,
                       msg.Sender, parameter => InitConnection(s, cm), parameter => s.Shutdown(SocketShutdown.Both)));
            });
        }

        public void InitConnection(Socket s, ConnectionModel cm)
        {
            // TODO: Check history and load it
            Thread t = new Thread(new ThreadStart(() => HandleMessages(s)));
            Model.CurrentConnection = cm;
            //Model.Connections.Add();
        }

        private void HandleMessages(Socket s)
        {
            while (s.Connected)
            {

            }
        }
       
        public void ConnectCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            try
            {
                client.EndConnect(ar);
            } catch (Exception e) // TODO: Narrower catch. Check what it throws and when
            {
                Console.WriteLine(e);
            }

            RequestConnectMessage msg = new RequestConnectMessage(this.Model.Username);
            string strMsg = JsonConvert.SerializeObject(msg) + "<EOM>";
            client.Send(Encoding.UTF8.GetBytes(strMsg));
        }

        public void Connect(string IP, string Port)
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
                tempSocket.BeginConnect(ipe, new AsyncCallback(ConnectCallback), tempSocket);
            } catch(SocketException) // TODO: Check what it throws and when
            {
                throw new InvalidIPException("Connection could not be made");
            }
        }

        private void StartConnection()
        {

        }
    }
}
