using System;
using System.Collections.Generic;
using System.Text;

namespace UKFast.API.Client
{
    public class UKFastAPIKeyCredentials : UKFastCredentials
    {
        public string APIKey { get; private set; }

        public UKFastAPIKeyCredentials(string apiKey)
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
