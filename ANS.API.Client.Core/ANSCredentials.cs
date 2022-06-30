using System;
using System.Collections.Generic;
using System.Text;

namespace ANS.API.Client
{
    public abstract class ANSCredentials
    {
        public abstract Dictionary<string, IEnumerable<string>> GetAuthHeaders();
    }
}
