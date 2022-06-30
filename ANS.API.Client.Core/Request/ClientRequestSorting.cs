using System;
using System.Collections.Generic;
using System.Text;

namespace ANS.API.Client.Request
{
    public class ClientRequestSorting
    {
        public string Property { get; set; }
        public bool Descending { get; set; }

        public ClientRequestSorting Copy()
        {
            return (ClientRequestSorting)this.MemberwiseClone();
        }
    }
}
