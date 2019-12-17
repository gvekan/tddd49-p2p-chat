using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TDDD49.Models;

namespace TDDD49.Messages
{
    [JsonObject(MemberSerialization.OptIn)]
    class ImageChatMessage : MessageBase
    {
        public ImageChatMessage(ImageMessageModel ImageMessage, string Sender, Guid id) : base(Sender, id, "ImageChat")
        {
            this.Image = ImageMessage.Image;
        }

        [JsonConstructor]
        public ImageChatMessage(byte[] ImageData, string Sender, Guid id) : base(Sender, id, "ImageChat")
        {
            this.ImageData = ImageData;
        }

        public BitmapImage Image
        {
            get
            {
                using (var ms = new MemoryStream(ImageData))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad; // here
                    image.StreamSource = ms;
                    image.EndInit();
                    return image;
                }
            }
            set
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(value));
                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    ImageData = ms.ToArray();
                }
            }
        }

        [JsonProperty]
        public byte[] ImageData
        {
            get;
            set;
        }
    }
}