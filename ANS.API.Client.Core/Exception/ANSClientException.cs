using System;
using System.Collections.Generic;
using System.Text;

namespace ANS.API.Client.Exception
{
    public class ANSClientException : System.Exception
    {
        public ANSClientException() { }
        public ANSClientException(string message) : base(message) { }
        public ANSClientException(string message, System.Exception ex) : base(message, ex) { }
    }
}
