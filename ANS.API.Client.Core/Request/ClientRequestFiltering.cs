using System;
using System.Collections.Generic;
using System.Text;

namespace ANS.API.Client.Request
{
    public enum FilteringOperator
    {
        EQ,
        NEQ,
        LK,
        NLK,
        GT,
        LT,
        IN,
        NIN
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
