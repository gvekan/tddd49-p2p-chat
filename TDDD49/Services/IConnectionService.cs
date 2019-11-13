using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace TDDD49.Services
{
    interface IConnectionService
    {
        void StartListen();

        void HandleConnection(Socket s);

        void Connect(string IP, string Port);
    }
}
