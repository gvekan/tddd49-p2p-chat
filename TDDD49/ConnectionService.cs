using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TDDD49
{
    class ConnectionService
    {
        private IPEndPoint IP;
        private Thread listen;
        ConnectionService(IPEndPoint IP)
        {
            this.IP = IP;
        }

        public int Port
        {
            set
            {
                if (IP.Port != value)
                {
                    IP.Port = value;
                    listen.Interrupt();
                    StartListen();
                }
            }
        }

        public void StartListen()
        {
            listen = new Thread(new ThreadStart(Listen));
        }

        public void Listen()
        {
            Socket listenSocket = new Socket(AddressFamily.InterNetwork,
                                     SocketType.Stream,
                                     ProtocolType.Tcp);
            listenSocket.Bind(IP);
            listenSocket.Listen(10);
            while (true)
            {
                Socket conSocket = listenSocket.Accept();
                // TODO: Save thread somewhere
                new Thread(new ThreadStart(() => HandleMessages(conSocket)));
            }
        }

        public void HandleMessages(Socket s)
        {

        }

       

        private Socket Connect(string IP, string Port)
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
                    s = tempSocket;
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
