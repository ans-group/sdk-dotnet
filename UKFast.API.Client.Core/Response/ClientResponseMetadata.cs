using System;
using System.Collections.Generic;
using System.Text;

namespace UKFast.API.Client.Response
{
    public class ClientResponseMetadata
    {
        [Newtonsoft.Json.JsonProperty("pagination")]
        public ClientResponseMetadataPagination Pagination { get; set; }

        [Newtonsoft.Json.JsonProperty("location")]
        public string Location { get; set; }
    }
}
