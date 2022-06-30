using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ANS.API.Client.Response;

namespace ANS.API.Client.Exception
{
    public class ANSClientRequestException : ANSClientException
    {
        public int StatusCode { get; set; }
        public IEnumerable<ClientResponseError> Errors { get; set; }

        public ANSClientRequestException(int statusCode, IEnumerable<ClientResponseError> errors) : base(GetErrorMessage(statusCode, errors))
        {
            this.StatusCode = statusCode;
            this.Errors = errors;
        }

        public ANSClientRequestException(int statusCode, IEnumerable<ClientResponseError> errors, System.Exception innerEx) : base(GetErrorMessage(statusCode, errors), innerEx)
        {
            this.StatusCode = statusCode;
            this.Errors = errors;
        }

        public ANSClientRequestException(int statusCode, string rawResponse) : base(GetErrorMessage(statusCode, rawResponse))
        {
            this.StatusCode = statusCode;
        }

        public ANSClientRequestException(int statusCode, string rawResponse, System.Exception innerEx) : base(GetErrorMessage(statusCode, rawResponse), innerEx)
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