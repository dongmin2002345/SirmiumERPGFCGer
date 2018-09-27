using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppCommonCode.Helpers
{
    public class CustomWebClient : WebClient
    {
        #region Timeout
        private int _Timeout;

        public int Timeout
        {
            get { return _Timeout; }
            set
            {
                if (_Timeout != value)
                {
                    _Timeout = value;
                }
            }
        }
        #endregion

        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = Timeout * 1000;
            return w;
        }
    }
}
