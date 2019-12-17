using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49.Messages
{
    class BuzzMessage : MessageBase
    {
        public BuzzMessage(string Sender, Guid id, string Type = "Buzz") : base(Sender, id, Type)
        {
        }
    }
}
