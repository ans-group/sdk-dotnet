using System;
using System.Collections.Generic;
using System.Text;

namespace UKFast.API.Client.Request
{
    public enum FilteringOperator
    {
        EQ,
        NEQ,
        LK,
        GT,
        LT,
        IN
    }

    public class ClientRequestFiltering
    {
        public string Property { get; set; }
        public FilteringOperator Operator { get; set; }
        public string[] Value { get; set; }

        public ClientRequestFiltering Copy()
        {
            return (ClientRequestFiltering)this.MemberwiseClone();
        }
    }
}
