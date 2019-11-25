using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49.Messages
{
    class AcceptDeclineMessage : MessageBase
    {
        public AcceptDeclineMessage(Boolean IsAccepted, string Sender) : base("", Sender, "AcceptDecline")
        {
            this.IsAccepted = IsAccepted;
        }

        public Boolean IsAccepted
        {
            get; set;
        }
    }
}
