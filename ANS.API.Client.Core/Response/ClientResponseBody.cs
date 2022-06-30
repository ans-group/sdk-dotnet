using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Converters;

namespace ANS.API.Client.Response
{
    public class ClientResponseBody<T>
    {
        /// <summary>
        /// Response metadata
        /// </summary>
        [Newtonsoft.Json.JsonProperty("meta")]
        public ClientResponseMetadata Metadata { get; set; }

        /// <summary>
        /// An array of errors, if applicable
        /// </summary>
        [Newtonsoft.Json.JsonProperty("errors")]
        public IEnumerable<ClientResponseError> Errors { get; set; }

        [Newtonsoft.Json.JsonProperty("data")]
        public T Data { get; set; }

        /// <summary>
        /// Returns whether response is paginated
        /// </summary>
        /// <returns></returns>
        public bool IsPaginated()
        {
            return (this.Metadata?.Pagination != null);
        }
    }
}
