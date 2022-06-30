using System;
using System.Collections.Generic;
using System.Text;
using ANS.API.Client.Response;

namespace ANS.API.Client.Exception
{
    public class ANSClientNotFoundRequestException : Client.Exception.ANSClientRequestException
    {
        public ANSClientNotFoundRequestException(IEnumerable<ClientResponseError> errors) : base(404, errors) { }
    }
}
