using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UKFast.API.Client.Response
{
    public class ClientResponseBase
    {
        public int StatusCode { get; set; }
        public Dictionary<string, IEnumerable<string>> Headers { get; set; }

        protected bool IsValid(IEnumerable<int> expectedStatusCodes = null)
        {
            return (expectedStatusCodes != null && expectedStatusCodes.Contains(this.StatusCode))
                || (expectedStatusCodes == null && IsSuccessStatusCode());
        }

        protected bool IsSuccessStatusCode()
        {
            return (this.StatusCode >= 200) && (this.StatusCode <= 299);
        }
    }
}
