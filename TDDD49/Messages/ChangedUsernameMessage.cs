using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49.Messages
{
    class ChangedUsernameMessage : MessageBase
    {
        public ChangedUsernameMessage(string Sender, Guid id) : base(Sender, id, "ChangedUsername")
        {
        }
    }
}
