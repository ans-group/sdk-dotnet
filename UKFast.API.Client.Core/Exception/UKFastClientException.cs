using System;
using System.Collections.Generic;
using System.Text;

namespace UKFast.API.Client.Exception
{
    public class UKFastClientException : System.Exception
    {
        public UKFastClientException() { }
        public UKFastClientException(string message) : base(message) { }
    }
}
