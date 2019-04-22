using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UKFast.API.Client.Request;
using UKFast.API.Client.Response;

namespace UKFast.API.Client.Models
{
    public class Paginated<T> where T : ModelBase
    {
        private IUKFastClient Client { get; }
        private string Resource { get; }
        private ClientRequestParameters Parameters { get; }
        private ClientResponseMetadataPagination Pagination { get; }

        public IList<T> Items { get; set; }

        public int TotalPages
        {
            get
            {
                return this.Pagination.TotalPages;
            }
        }

        public int CurrentPage
        {
            get
            {
                return this.Pagination.CurrentPage;
            }
        }

        public Paginated(IUKFastClient client, string resource, ClientRequestParameters parameters, ClientResponse<IList<T>> response)
        {
            this.Client = client;
            this.Resource = resource;
            this.Parameters = parameters ?? new ClientRequestParameters();
            this.Items = response?.Body?.Data;
            this.Pagination = response?.Body?.Metadata?.Pagination ?? new ClientResponseMetadataPagination();
        }

        public async Task<Paginated<T>> Next()
        {
            if (this.CurrentPage >= this.TotalPages)
            {
                return null;
            }

            ClientRequestParameters parameters = this.Parameters.DeepCopy();
            parameters.Pagination.Page = this.CurrentPage + 1;

            return await this.Client.GetPaginatedAsync<T>(this.Resource, parameters);
        }

        public async Task<Paginated<T>> Previous()
        {
            if (this.CurrentPage <= 1)
            {
                return null;
            }

            ClientRequestParameters parameters = this.Parameters.DeepCopy();
            parameters.Pagination.Page = this.CurrentPage - 1;

            return await this.Client.GetPaginatedAsync<T>(this.Resource, parameters);
        }

        public async Task<Paginated<T>> First()
        {
            ClientRequestParameters parameters = this.Parameters.DeepCopy();
            parameters.Pagination.Page = 1;

            return await this.Client.GetPaginatedAsync<T>(this.Resource, parameters);
        }

        public async Task<Paginated<T>> Last()
        {
            ClientRequestParameters parameters = this.Parameters.DeepCopy();
            parameters.Pagination.Page = this.TotalPages;

            return await this.Client.GetPaginatedAsync<T>(this.Resource, parameters);
        }
    }
}
