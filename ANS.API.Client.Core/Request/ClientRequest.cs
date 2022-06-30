using System;
using System.Collections.Generic;
using System.Text;
using ANS.API.Client.Models;

namespace ANS.API.Client.Request
{
    public class ClientRequest : ClientRequestBase
    {
        public object Body { get; set; }
        public ClientRequestParameters Parameters { get; set; }

        /// <summary>
        /// Examines pagination and adds relevant pagination query parameters to request
        /// </summary>
        public void HydratePaginationQuery()
        {
            if (this.Parameters?.Pagination != null)
            {
                if (this.Parameters.Pagination.Page > 0 && !this.Query.ContainsKey("page"))
                {
                    this.Query.Add("page", this.Parameters.Pagination.Page.ToString());
                }

                if (this.Parameters.Pagination.PerPage > 0 && !this.Query.ContainsKey("per_page"))
                {
                    this.Query.Add("per_page", this.Parameters.Pagination.PerPage.ToString());
                }
            }
        }

        /// <summary>
        /// Examines sorting and adds relevant sorting query parameters to request
        /// </summary>
        /// <param name="request">Instance of ClientRequest object</param>
        public void HydrateSortingQuery()
        {
            if (string.IsNullOrEmpty(this.Parameters?.Sorting?.Property) != true)
            {
                this.Query.Add("sort", $"{this.Parameters.Sorting.Property}:" + ((this.Parameters.Sorting.Descending) ? "desc" : "asc"));
            }
        }

        /// <summary>
        /// Examines filtering and adds relevant filtering query parameters to request
        /// </summary>
        /// <param name="request">Instance of ClientRequest object</param>
        public void HydrateFilteringQuery()
        {
            if (this.Parameters?.Filtering?.Count > 0)
            {
                foreach (ClientRequestFiltering filter in this.Parameters.Filtering)
                {
                    this.Query.Add($"{filter.Property}:{filter.Operator.ToString().ToLower()}", string.Join(",", filter.Value));
                }
            }
        }

        public ClientRequest()
        {
            this.Parameters = new ClientRequestParameters();
        }
    }
}
