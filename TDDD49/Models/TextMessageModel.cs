using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49.Models
{
    class TextMessageModel : MessageModel
    {

        public TextMessageModel(string _Text, bool IsSender) : base(IsSender)
        {
            Text = _Text;
        }

        public string Text
        {
            get;
            set;
        }
    }
}
