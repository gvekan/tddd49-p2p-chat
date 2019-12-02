using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49.Exceptions
{
    class NoConnectionException : Exception
    {
        private String _Message;

        public NoConnectionException() : base()
        {

            this._Message = "No connection";
        }

        public NoConnectionException(string Message) : base(Message)
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
