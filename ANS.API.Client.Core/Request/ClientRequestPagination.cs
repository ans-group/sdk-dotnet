using System;
using System.Collections.Generic;
using System.Text;

namespace ANS.API.Client.Request
{
    public class ClientRequestPagination
    {
        public int PerPage { get; set; }
        public int Page { get; set; }

        public ClientRequestPagination Copy()
        {
            return (ClientRequestPagination)this.MemberwiseClone();
        }
    }
}
