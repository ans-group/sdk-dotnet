using System;
using System.Collections.Generic;
using System.Text;

namespace UKFast.API.Client.Operations
{
    public class OperationsBase<T> where T : IUKFastClient
    {
        public T Client { get; }

        public OperationsBase(T client) => this.Client = client;
    }
}
