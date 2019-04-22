using System;
using System.Collections.Generic;
using System.Text;
using UKFast.API.Client.Response;

namespace UKFast.API.Client.Exception
{
    public class UKFastClientNotFoundRequestException : Client.Exception.UKFastClientRequestException
    {
        public UKFastClientNotFoundRequestException(int statusCode, IEnumerable<ClientResponseError> errors) : base(statusCode, errors) { }
    }
}
