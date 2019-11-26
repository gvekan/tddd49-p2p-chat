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
using TDDD49.Exceptions;

namespace TDDD49.Services
{ 
    public class StateObject
    {
        public Socket WorkSocket = null;
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
        private bool Running = true;
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
                Stop();
                Running = true;
            }

            IP = new IPEndPoint(IPAddress.Any, Model.Port);
            ListenThread = new Thread(new ThreadStart(() => Listen()));
            ListenThread.Start();
        }

        private void Listen()
        {
            ListenSocket = new Socket(AddressFamily.InterNetwork,
                                     SocketType.Stream,
                                     ProtocolType.Tcp);
            try
            {
                ListenSocket.Bind(this.IP);
            } catch (Exception e)
            {
                Actions.HandleBugException(e, "Something went wrong with the connection. Try changing the port.");
                return;
            }

            ListenSocket.Listen(10);
            while (Running)
            {
                Socket s = null;
                try
                {
                    s = ListenSocket.Accept();
                } catch (SocketException e)
                {
                    if (Running)
                    {
                        Running = false;
                        Actions.HandleBugException(e, "Something went really wrong. Try to restart the application.");
                    }
                    // else we created the exception when closing.
                }

                if (s == null) continue;

                // TODO: Lock on ConSocket
                if (ConSocket == null)
                {
                    ConSocket = s;
                    InitConnection(s);
                } else
                {
                    SendAcceptDeclineMessage(s, false);
                    s.Shutdown(SocketShutdown.Both);
                }
            }
        }

        // WHen we get a connection
        private void InitConnection(Socket s)
        {
            // Read info here
            RequestConnectMessage msg = (RequestConnectMessage)JsonConvert.DeserializeObject(ReceiveMessage(s), typeof(RequestConnectMessage));
            string ip = s.RemoteEndPoint.ToString(); // TODO: change port to RequestConnectMessage.Port
            ConnectionModel cm = new ConnectionModel(ip, msg.Sender, null);
            Application.Current.Dispatcher.Invoke(() =>
            {
                Actions.OpenDialog(typeof(AcceptDeclineDialog),
                    new AcceptDeclineDialogViewModel(ip,
                       msg.Sender, parameter => AcceptConnection(s, cm), parameter => DeclineConnection(s)));
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
            StartRecieve(s);
        }

        public void DeclineConnection(Socket s)
        {
            SendAcceptDeclineMessage(s, false);
            OnDisconnect();
        }


        // When user tries to connect to another user
        public void Connect(string IP, string Port)
        {
            IPAddress IPAddr;
            IPEndPoint ipe;
            try
            {
                IPAddress.TryParse(IP, out IPAddr);
                if (IPAddr == null)
                    throw new InvalidIPException();
                ipe = new IPEndPoint(IPAddr, Convert.ToInt32(Port));
            }
            catch (Exception ex) when(ex is SocketException || ex is ArgumentOutOfRangeException)
            {
                throw new InvalidIPException("Connection could not be made");
            }
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

            if (!client.Connected)
            {
                OnDisconnect();
                Actions.HandleBugException(new NoConnectionException(), "Other user did not accept.");
            }

            RequestConnectMessage msg = new RequestConnectMessage(this.Model.Username);
            string strMsg = msg.Serialize();
            client.Send(Encoding.UTF8.GetBytes(strMsg));

            AcceptDeclineMessage acceptDeclineMessage = (AcceptDeclineMessage)JsonConvert.DeserializeObject(ReceiveMessage(client), typeof(AcceptDeclineMessage));
            if (acceptDeclineMessage.IsAccepted)
            {
                string ip = client.RemoteEndPoint.ToString(); // TODO: change port to RequestConnectMessage.Port
                Application.Current.Dispatcher.Invoke(() => Model.CurrentConnection = new ConnectionModel(ip,
                       acceptDeclineMessage.Sender, null));
                StartRecieve(client);
            }
            else
            {
                OnDisconnect();
            }

        }

        private void StartRecieve(Socket s)
        {
            StateObject so = new StateObject();
            so.WorkSocket = s;

            DoBeginRecieve(so);
        }

        private void DoBeginRecieve(StateObject so)
        {
            if (!so.WorkSocket.Connected) return;
            try
            {
                so.WorkSocket.BeginReceive(so.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(RecieveCallback), so);
            } catch (Exception ex) when(ex is SocketException || ex is ObjectDisposedException)
            {
                OnDisconnect();
                MessageBox.Show("Other user was disconnected.");
            }
        }

        private void RecieveCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            string content = string.Empty;
            if (!state.WorkSocket.Connected)
                return;
            int bytesRead = state.WorkSocket.EndReceive(ar);

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
            DoBeginRecieve(state);
        }

        public void Disconnect()
        {
            if (ConSocket.Connected)
                ConSocket.Send(Encoding.UTF8.GetBytes(new DisconnectMessage(Model.Username).Serialize()));
            OnDisconnect();
        }

        private void OnDisconnect()
        {
            // TODO: (Not here) What if other chat party exits unexpectedly, Check socket timeout/connected
            try
            {
                if(ConSocket != null && ConSocket.Connected)
                    ConSocket.Shutdown(SocketShutdown.Both);
            } finally
            {
                ConSocket.Close();
            }
            ConSocket = null;
        }

        public void OnExit(object sender, ExitEventArgs e)
        {
            Stop();
        } 

        private void Stop()
        {
            Running = false;
            ListenSocket.Close();
            if (ConSocket != null)
                Disconnect();
            ListenThread.Abort();
        }

        private void HandleMessage(string Message)
        {
            dynamic msg = JsonConvert.DeserializeObject(Message);
            
            if (msg.Type == "Chat")
            {
            }
            if(msg.Type == "Disconnect")
            {
                OnDisconnect();
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
