using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49.Messages
{
    class MessageBase
    {
        public MessageBase(string Sender, Guid id, string Type = "Chat")
        {
            this.Type = Type;
            this.Sender = Sender;
            this.id = id;
        }
        public string Type
        {
            get; set;
        }

        public Guid id
        {
            get;
            set;
        }

        public string Sender
        {
            get; set;
        }

        public string SendTime {
            get; set;
        }

        public string Serialize()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;
            settings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full;
            return JsonConvert.SerializeObject(this, this.GetType(), settings) + "<EOM>";
        }
    }
}
