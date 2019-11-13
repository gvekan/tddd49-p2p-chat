using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDDD49.Helpers;

namespace TDDD49.Models
{
    class MainModel : UserModel
    {
        public class MainModelParams : UserModelParams
        {

        }

        #region Private Fields
        // TODO: Add list of connections
        #endregion

        public MainModel(MainModelParams Params) : base(Params)
        {

        }
    }
}
