using System;
using System.Collections.Generic;
using System.Text;

namespace UKFast.API.Client
{
    public abstract class UKFastCredentials
    {
        public abstract Dictionary<string, IEnumerable<string>> GetAuthHeaders();
    }
}
