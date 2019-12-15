using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Windows;
using TDDD49.Models;

namespace TDDD49.Services
{
    interface IConnectionService
    {
        void StartListen();

        void Connect(string IP, string Port);

        void Disconnect();

        void OnExit(object sender, ExitEventArgs e);

        void Send(MessageModel Message);
    }
}
