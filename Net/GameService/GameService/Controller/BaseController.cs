using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using GameService.Servers;
namespace GameService.Controller
{
    abstract class BaseController
    {
        protected RequestCode requestCode = RequestCode.None;

        public virtual string DefaultHandle(string data,Client client,Server server) { return null; }
        public RequestCode RequesCode
        {
            get
            {
                return requestCode;
            }
        }
    }
}
