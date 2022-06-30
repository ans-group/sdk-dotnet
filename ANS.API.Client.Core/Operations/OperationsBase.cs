using System;
using System.Collections.Generic;
using System.Text;

namespace ANS.API.Client.Operations
{
    public class OperationsBase<T> where T : IANSClient
    {
        public T Client { get; }

        public OperationsBase(T client) => this.Client = client;
    }
}
