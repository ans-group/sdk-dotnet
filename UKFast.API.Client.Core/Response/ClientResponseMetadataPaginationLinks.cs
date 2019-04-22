using System;
using System.Collections.Generic;
using System.Text;

namespace UKFast.API.Client.Response
{
    public class ClientResponseMetadataPaginationLinks
    {
        [Newtonsoft.Json.JsonProperty("first")]
        public string FirstPage { get; set; }

        [Newtonsoft.Json.JsonProperty("previous")]
        public string Previous { get; set; }

        [Newtonsoft.Json.JsonProperty("next")]
        public string NextPage { get; set; }

        [Newtonsoft.Json.JsonProperty("last")]
        public string LastPage { get; set; }
    }
}
