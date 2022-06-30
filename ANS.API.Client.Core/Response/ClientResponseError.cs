using System;
using System.Collections.Generic;
using System.Text;

namespace ANS.API.Client.Response
{
    public class ClientResponseError
    {
        [Newtonsoft.Json.JsonProperty("title")]
        public string Title { get; set; }

        [Newtonsoft.Json.JsonProperty("detail")]
        public string Detail { get; set; }

        [Newtonsoft.Json.JsonProperty("status")]
        public int Status { get; set; }

        [Newtonsoft.Json.JsonProperty("source")]
        public string Source { get; set; }

        public override string ToString()
        {
            return $"Title=[{this.Title}] Detail=[{this.Detail}] Status=[{this.Status}] Source=[{this.Source}]";
        }
    }
}
