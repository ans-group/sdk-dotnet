using System;
using System.Collections.Generic;
using System.Text;

namespace UKFast.API.Client.Exception
{
    public class UKFastClientValidationException : UKFastClientException
    {
        public UKFastClientValidationException() { }
        public UKFastClientValidationException(string message) : base(message) { }
    }
}
