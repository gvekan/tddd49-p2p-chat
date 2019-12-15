using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDDD49.Models;

namespace TDDD49.Messages
{
    class ImageChatMessage : MessageBase
    {
        public ImageChatMessage(ImageMessageModel ImageMessage, string Sender, Guid id) : base(Sender, id, "ImageChat")
        {
           // TODO: Set image
        }
    }
}
