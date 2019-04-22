using System;
using System.Collections.Generic;
using System.Text;
using UKFast.API.Client.Response;

namespace UKFast.API.Client.Exception
{
    public class UKFastClientNotFoundRequestException : Client.Exception.UKFastClientRequestException
    {
        public UKFastClientNotFoundRequestException(IEnumerable<ClientResponseError> errors) : base(404, errors) { }
    }
}
