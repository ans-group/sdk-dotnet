using System;
using System.Collections.Generic;
using System.Text;

namespace ANS.API.Client
{
    public class ANSAPIKeyCredentials : ANSCredentials
    {
        public string APIKey { get; private set; }

        public ANSAPIKeyCredentials(string apiKey)
        {
            this.APIKey = apiKey;
        }

        public override Dictionary<string, IEnumerable<string>> GetAuthHeaders()
        {
            List<string> value = new List<string>();
            value.Add(this.APIKey);

            return new Dictionary<string, IEnumerable<string>>()
            {
                { "Authorization", value }
            };
        }
    }
}
