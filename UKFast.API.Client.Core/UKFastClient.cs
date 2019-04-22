using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UKFast.API.Client.Models;
using UKFast.API.Client.Request;
using UKFast.API.Client.Response;
using static UKFast.API.Client.ClientConnection;

namespace UKFast.API.Client
{
    public class UKFastClient : IUKFastClient
    {
        protected const int PAGINATION_DEFAULT_PERPAGE = 15;
        public IConnection Connection { get; }
        public ClientConfig Config { get; }

        public delegate Task<Paginated<T>> GetPaginatedAsyncFunc<T>(ClientRequestParameters parameters) where T : ModelBase;

        public UKFastClient(IConnection connection)
        {
            this.Connection = connection;
            this.Config = GetDefaultConfig();
        }

        public UKFastClient(IConnection connection, ClientConfig config)
        {
            this.Connection = connection;
            this.Config = config;
        }

        public async Task<IList<T>> GetAllAsync<T>(GetPaginatedAsyncFunc<T> func, ClientRequestParameters parameters = null) where T : ModelBase
        {
            parameters = InitialisePaginationParameters(parameters);
            List<T> items = new List<T>();

            Paginated<T> p = await func(parameters);
            if (p.Items?.Count > 0)
            {
                items.AddRange(p.Items);
            }

            while (true)
            {
                p = await p.Next();

                if (p == null)
                {
                    break;
                }

                if (p.Items?.Count > 0)
                {
                    items.AddRange(p.Items);
                }
            }

            return items;
        }

        public async Task<Paginated<T>> GetPaginatedAsync<T>(string resource, ClientRequestParameters parameters = null) where T : ModelBase
        {
            parameters = InitialisePaginationParameters(parameters);

            ClientResponse<IList<T>> response = await GetResponseAsync<IList<T>>(resource, parameters);

            return new Paginated<T>(this, resource, parameters, response);
        }

        public async Task<IList<T>> GetListAsync<T>(string resource, ClientRequestParameters parameters = null)
        {
            ClientResponse<IList<T>> response = await GetResponseAsync<IList<T>>(resource, parameters);
            return response.Body.Data;
        }

        public async Task<T> GetAsync<T>(string resource, ClientRequestParameters parameters = null)
        {
            ClientResponse<T> response = await GetResponseAsync<T>(resource, parameters);
            return response.Body.Data;
        }

        protected virtual async Task<ClientResponse<T>> GetResponseAsync<T>(string resource, ClientRequestParameters parameters = null)
        {
            ClientResponse<T> response = await this.Connection.GetAsync<T>(resource, parameters);
            response.Validate();

            return response;
        }

        protected virtual ClientConfig GetDefaultConfig()
        {
            return new ClientConfig();
        }

        private ClientRequestParameters InitialisePaginationParameters(ClientRequestParameters parameters)
        {
            parameters = parameters ?? new ClientRequestParameters();
            parameters.Pagination = parameters.Pagination ?? new ClientRequestPagination()
            {
                Page = 1,
                PerPage = this.Config.PaginationDefaultPerPage
            };

            return parameters;
        }
    }
}
