﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49.Models
{
    abstract class MessageModel
    { 
        public MessageModel(bool _IsSender) 
        {
            IsSender = _IsSender;
        }

        public bool IsSender
        {
            get;
            set;
        }

    }


}
