using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49.Messages
{
    class AcceptDeclineMessage : MessageBase
    {
        public AcceptDeclineMessage(bool IsAccepted, string Sender, Guid id) : base(Sender, id, "AcceptDecline")
        {
            this.IsAccepted = IsAccepted;
        }

        public bool IsAccepted
        {
            get; set;
        }
    }
}
