using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49
{
    class InvalidIPException : Exception
    {
        private String _Message;

        public InvalidIPException() : base()
        {
            
            this._Message = "Invalid IP";
        }

        public InvalidIPException(string Message) : base(Message)
        {
            this._Message = Message;
        }

        override
        public string ToString()
        {
            return _Message;
        }

        override
        public string Message
        {
            get { return _Message; }
        }

    }
}
