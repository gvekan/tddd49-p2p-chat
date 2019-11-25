using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49.Messages
{
    class MessageBase
    {
        public MessageBase()
        {

        }

        public string Message
        {
            get; set;
        }

        public string Sender
        {
            get; set;
        }

        public string SendTime {
            get; set;
        }
    }
}
