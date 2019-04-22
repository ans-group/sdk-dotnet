using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UKFast.API.Client.Response
{
    public class ClientResponse<T> : ClientResponseBase
    {
        public ClientResponseBody<T> Body { get; set; }

        public void Validate(IEnumerable<int> expectedStatusCodes = null)
        {
            if (!IsValid(expectedStatusCodes))
            {
                if (this.StatusCode == 404)
                {
                    throw new Exception.UKFastClientNotFoundRequestException(this.Body?.Errors);
                }
                else
                {
                    throw new Exception.UKFastClientRequestException(this.StatusCode, this.Body?.Errors);
                }
            }
        }
    }
}
