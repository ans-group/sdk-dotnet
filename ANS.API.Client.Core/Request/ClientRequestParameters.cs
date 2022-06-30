using System;
using System.Collections.Generic;
using System.Text;

namespace ANS.API.Client.Request
{
    public class ClientRequestParameters
    {
        public List<ClientRequestFiltering> Filtering { get; set; }
        public ClientRequestSorting Sorting { get; set; }
        public ClientRequestPagination Pagination { get; set; }

        public ClientRequestParameters()
        {
            this.Filtering = new List<ClientRequestFiltering>();
        }

        public ClientRequestParameters WithFiltering(ClientRequestFiltering filter)
        {
            if (filter != null)
            {
                this.Filtering.Add(filter);
            }

            return this;
        }

        public ClientRequestParameters WithFiltering(IEnumerable<ClientRequestFiltering> filters)
        {
            if (filters != null)
            {
                this.Filtering.AddRange(filters);
            }

            return this;
        }

        public ClientRequestParameters WithSorting(ClientRequestSorting sorting)
        {
            this.Sorting = sorting;
            return this;
        }

        public ClientRequestParameters WithPagination(ClientRequestPagination pagination)
        {
            this.Pagination = pagination;
            return this;
        }

        public ClientRequestParameters Copy()
        {
            return (ClientRequestParameters)this.MemberwiseClone();
        }

        public ClientRequestParameters DeepCopy()
        {
            ClientRequestParameters newParameters = Copy();

            newParameters.Filtering = new List<ClientRequestFiltering>();
            foreach (ClientRequestFiltering filter in this.Filtering)
            {
                newParameters.Filtering.Add(filter.Copy());
            }

            newParameters.Sorting = this.Sorting?.Copy();
            newParameters.Pagination = this.Pagination?.Copy();

            return newParameters;
        }
    }
}
