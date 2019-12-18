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
        [JsonConstructor]
        public ImageMessageModel(string Path, bool _IsSender) : base(_IsSender)
        {
            this.Path = Path;
        }

        [JsonProperty]
        public string Path
        {
            get;
            set;
        }

        public BitmapImage Image
        {
            get
            {
                return new BitmapImage(new Uri(Path));
            }
        }
    }
}
