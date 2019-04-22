using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UKFast.API.Client.Response;

namespace UKFast.API.Client.Exception
{
    public class UKFastClientRequestException : UKFastClientException
    {
        public int StatusCode { get; set; }
        public IEnumerable<ClientResponseError> Errors { get; set; }

        public UKFastClientRequestException(int statusCode, IEnumerable<ClientResponseError> errors) : base(GetErrorMessage(statusCode, errors))
        {
            this.StatusCode = statusCode;
            this.Errors = errors;
        }

        public UKFastClientRequestException(int statusCode, string rawResponse) : base(GetErrorMessage(statusCode, rawResponse))
        {
            this.StatusCode = statusCode;
        }

        private static string GetErrorMessage(int statusCode, IEnumerable<ClientResponseError> errors)
        {
            return $"Request failed with status code [{statusCode}]: " + ((errors != null) ? string.Join("; ", errors.Select(x => x.ToString())) : null);
        }

        private static string GetErrorMessage(int statusCode, string rawResponse)
        {
            return $"Request failed with status code [{statusCode}]: " + rawResponse;
        }
    }
}