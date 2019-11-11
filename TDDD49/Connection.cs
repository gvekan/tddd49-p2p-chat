using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TDDD49
{
    class Connection
    {
        private Thread Thread = null;
        private Socket Socket = null;

        public Connection(Thread t, Socket s)
        {
            this.Thread = t;
            this.Socket = s;
        }
    }
}
