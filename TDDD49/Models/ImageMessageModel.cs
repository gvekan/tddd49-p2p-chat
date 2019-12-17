using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;

namespace TDDD49.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    class ImageMessageModel : MessageModel
    {
        //private BitmapImage BMP;
        public ImageMessageModel(BitmapImage _Image, bool _IsSender) : base(_IsSender)
        {
            this.Image = _Image;
        }

        [JsonConstructor]
        public ImageMessageModel(byte[] ImageData, bool _IsSender) : base(_IsSender)
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

        /* JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(img));
            using(MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }*/
    }
}
