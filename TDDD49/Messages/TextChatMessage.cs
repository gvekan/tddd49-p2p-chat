using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDDD49.Models;

namespace TDDD49.Messages
{
    class TextChatMessage : MessageBase
    {
        public TextChatMessage(TextMessageModel TextMessage, string Sender, Guid id) : base(Sender, id, "TextChat")
        {
            this.Message = TextMessage.Text;
        }

        [JsonConstructor]
        public TextChatMessage(string Text, string Sender, Guid id) : base(Sender, id, "TextChat")
        {
            this.Message = Text;
        }

        public string Message
        {
            get;
            set;
        }
    }
}
