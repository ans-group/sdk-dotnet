using System;
using System.Collections.Generic;
using System.Text;

namespace ANS.API.Client.Request
{
    public enum ClientRequestMethod
    {
        GET,
        POST,
        PUT,
        PATCH,
        DELETE
    }

    public class ClientRequestBase
    {
        public string Resource { get; set; }
        public ClientRequestMethod Method { get; set; }
        public Dictionary<string, string> Query { get; set; }

        public ClientRequestBase()
        {
            this.Query = new Dictionary<string, string>();
        }
    }
}
