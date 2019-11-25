using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace TDDD49.Services
{
    class MessageService
    {
        public Queue<Object> MessageQueue = new Queue<Object>();
        public Action Listener;
        private Socket Client;

        public MessageService(Action Listener, Socket Client)
        {
            this.Listener = Listener;
            this.Client = Client;
        }


    }
}
