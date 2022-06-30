using System;
using System.Collections.Generic;
using System.Text;

namespace ANS.API.Client.Response
{
    public class ClientResponseMetadataPagination
    {
        [Newtonsoft.Json.JsonProperty("total")]
        public int Total { get; set; }

        [Newtonsoft.Json.JsonProperty("count")]
        public int Count { get; set; }

        [Newtonsoft.Json.JsonProperty("per_page")]
        public int PerPage { get; set; }

        [Newtonsoft.Json.JsonProperty("current_page")]
        public int CurrentPage { get; set; }

        [Newtonsoft.Json.JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [Newtonsoft.Json.JsonProperty("links")]
        public ClientResponseMetadataPaginationLinks Links { get; set; }
    }
}
