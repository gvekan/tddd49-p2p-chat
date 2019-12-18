using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49.Messages
{
    class RequestConnectMessage : MessageBase
    {
        public RequestConnectMessage(String SenderUsername, Guid id) : base(SenderUsername, id, "RequestConnect")
        { 
        }
    }
}
