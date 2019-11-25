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
    public class StateObject
    {
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];

        public StringBuilder sb = new StringBuilder();

    }

    class ConnectionService : IConnectionService
    {
        #region Private Fields
        private IPEndPoint IP;
        private Thread ListenThread;
        private Socket ListenSocket;
        private Socket _ConSocket = null;
        private MainModel Model;
        #endregion

        public ConnectionService(MainModel Model)
        {
            // TODO: Set message
            this.Model = Model;
            Model.PropertyChanged += Model_PropertyChanged;
            StartListen();
        }

        public Socket ConSocket
        {
            get
            {
                return _ConSocket;
            }
            set
            {
                Model.Connected = value != null;
                _ConSocket = value;
            }
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

                // TODO: Lock on ConSocket
                if (ConSocket == null)
                {
                    ConSocket = conSocket;
                    InitConnection();
                } else
                {
                    conSocket.Shutdown(SocketShutdown.Both);
                }
            }
        }

        // WHen we get a connection
        private void InitConnection()
        {
            // Read info here
            RequestConnectMessage msg = (RequestConnectMessage)JsonConvert.DeserializeObject(ReceiveMessage(ConSocket), typeof(RequestConnectMessage));
            string ip = ConSocket.RemoteEndPoint.ToString(); // TODO: change port to RequestConnectMessage.Port
            ConnectionModel cm = new ConnectionModel(ip, msg.Sender, null);
            Application.Current.Dispatcher.Invoke(() =>
            {
                Actions.OpenDialog(typeof(AcceptDeclineDialog),
                    new AcceptDeclineDialogViewModel(ip,
                       msg.Sender, parameter => AcceptConnection(ConSocket, cm), parameter => DeclineConnection(ConSocket)));
            });
        }

        private void SendAcceptDeclineMessage(Socket s, bool IsAccepted)
        {
            AcceptDeclineMessage msg = new AcceptDeclineMessage(IsAccepted, this.Model.Username);
            s.Send(Encoding.UTF8.GetBytes(msg.Serialize()));
        }

        public void AcceptConnection(Socket s, ConnectionModel cm)
        {
            SendAcceptDeclineMessage(s, true);
            // TODO: Check history and load it
            Model.CurrentConnection = cm;
            StartRecieve();
        }

        public void DeclineConnection(Socket s)
        {
            SendAcceptDeclineMessage(s, false);
            s.Shutdown(SocketShutdown.Both);
        }


        // When user tries to connect to another user
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
            ConSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                ConSocket.BeginConnect(ipe, new AsyncCallback(ConnectCallback), ConSocket);
            } catch(SocketException) // TODO: Check what it throws and when
            {
                throw new InvalidIPException("Connection could not be made");
            }
        }

        public void ConnectCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            try
            {
                client.EndConnect(ar);
            }
            catch (Exception e) // TODO: Narrower catch. Check what it throws and when
            {
                Console.WriteLine(e);
            }

            RequestConnectMessage msg = new RequestConnectMessage(this.Model.Username);
            string strMsg = msg.Serialize();
            client.Send(Encoding.UTF8.GetBytes(strMsg));

            AcceptDeclineMessage acceptDeclineMessage = (AcceptDeclineMessage)JsonConvert.DeserializeObject(ReceiveMessage(client), typeof(AcceptDeclineMessage));
            if (acceptDeclineMessage.IsAccepted)
            {
                string ip = client.RemoteEndPoint.ToString(); // TODO: change port to RequestConnectMessage.Port
                Model.CurrentConnection = new ConnectionModel(ip,
                       acceptDeclineMessage.Sender, null);
                StartRecieve();
            }
            else
            {
                client.Shutdown(SocketShutdown.Both);
            }

        }

        private void StartRecieve()
        {
            StateObject so = new StateObject();

            ConSocket.BeginReceive(so.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(RecieveCallback), so);
        }

        private void RecieveCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            string content = string.Empty;
        
            int bytesRead = ConSocket.EndReceive(ar);

            if(bytesRead > 0)
            {
                state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));
                content = state.sb.ToString();
                int indexOfEOM = content.IndexOf("<EOM>");
                while (indexOfEOM > -1)
                {
                    String message = content.Substring(0, indexOfEOM); // Ignores EOM
                    HandleMessage(message);

                    content = content.Substring(indexOfEOM + 4); // Ignores EOM


                    indexOfEOM = content.IndexOf("<EOM>");
                }
                state.sb = new StringBuilder(content);

            }

            ConSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(RecieveCallback), state);
        }

        private void HandleMessage(string Message)
        {
            dynamic msg = JsonConvert.DeserializeObject(Message);
            
            if (msg.Type == "Chat")
            {
            }
            // TODO: Deseralize and do something with the message
        }

        private string ReceiveMessage(Socket s)
        {
            string buffer = "";
            byte[] bytes = new byte[1024];
            do
            {
                s.Receive(bytes);
                buffer += Encoding.UTF8.GetString(bytes).TrimEnd('\0');
                bytes = new byte[256];
            } while (buffer.Substring(buffer.Length - 5) != "<EOM>");
            return buffer.Remove(buffer.Length - 5);
        }
    }
}
