using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TDDD49.Helpers;
using TDDD49.ViewModel;
using TDDD49.Views;

namespace TDDD49
{
    public class ConnectionService
    {
        private IPEndPoint IP;
        private Thread listenThread;
        private int defaultPort = 61523;
        private List<Connection> connectionList = new List<Connection>(); // TODO: Temporary connection storage. Move to other place and use Events?

        public ConnectionService()
        {
                IP = new IPEndPoint(IPAddress.Any, defaultPort);
                StartListen();
        }

        public int Port
        {
            set
            {
                if (IP.Port != value)
                {
                    IP.Port = value;
                    listenThread.Interrupt();
                    StartListen();
                }
            }
        }

        public void StartListen()
        {
            listenThread = new Thread(new ThreadStart(Listen));
        }

        public void Listen()
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
                AcceptDialog dialog = new AcceptDialog();
                dialog.DataContext = new AcceptDialogViewModel(conSocket.RemoteEndPoint.ToString(), "Someone", () => HandleConnection(conSocket));
                // TODO: Save thread somewhere
            }
        }

        public void HandleConnection(Socket s)
        {
            Thread t = new Thread(new ThreadStart(() => HandleMessages(s)));
            connectionList.Add(new Connection(t, s));
        }

        public void HandleMessages(Socket s)
        {
            while (s.Connected)
            {

            }
        }
       

        public Socket Connect(string IP, string Port)
        {
            Socket s = null;
            IPHostEntry hostEntry = Dns.GetHostEntry(IP);
            foreach (IPAddress address in hostEntry.AddressList)
            {
                IPEndPoint ipe = new IPEndPoint(address, Convert.ToInt32(Port));
                Socket tempSocket =
                    new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                tempSocket.Connect(ipe);

                if (tempSocket.Connected)
                {
                    // TODO: SPAWN NEW THREAD
                    s = tempSocket;
                    HandleConnection(s);
                    break;
                }
                else
                {
                    continue;
                }
            }

            return s;
        }
    }
}
