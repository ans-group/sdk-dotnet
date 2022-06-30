using System;
using System.Collections.Generic;
using System.Text;

namespace ANS.API.Client.Exception
{
    public class ANSClientValidationException : ANSClientException
    {
        public ANSClientValidationException() { }
        public ANSClientValidationException(string message) : base(message) { }
    }
}
