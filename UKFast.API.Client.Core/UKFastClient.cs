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
        public IConnection Connection { get; }
        public ClientConfig Config { get; }

        /// <summary>
        /// GetPaginatedAsyncFunc defines a delegate for returning an instance of Paginated from given request parameters
        /// </summary>
        /// <typeparam name="T">Type of model</typeparam>
        /// <param name="parameters">Request parameters</param>
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

        /// <summary>
        /// GetAllAsync retrieves all pages for provided func
        /// </summary>
        /// <typeparam name="T">Type of model</typeparam>
        /// <param name="func">Implementation of GetPaginatedAsyncFunc</param>
        /// <param name="parameters">Request parameters</param>
        /// <returns></returns>
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

        /// <summary>
        /// GetPaginatedAsync returns an instance of Paginated, whilst validating the status code of the response
        /// </summary>
        public async Task<Paginated<T>> GetPaginatedAsync<T>(string resource, ClientRequestParameters parameters = null) where T : ModelBase
        {
            parameters = InitialisePaginationParameters(parameters);

            ClientResponse<IList<T>> response = await GetResponseAsync<IList<T>>(resource, parameters);

            return new Paginated<T>(this, resource, parameters, response);
        }

        /// <summary>
        /// GetAsync is a wrapper around the underlying Connection's GetAsync method, which validates the status code of the response
        /// </summary>
        public async Task<T> GetAsync<T>(string resource, ClientRequestParameters parameters = null)
        {
            ClientResponse<T> response = await GetResponseAsync<T>(resource, parameters);
            return GetDataOrDefault(response);
        }

        /// <summary>
        /// PostAsync is a wrapper around the underlying Connection's PostAsync method, which validates the status code of the response
        /// </summary>
        public async Task<T> PostAsync<T>(string resource, object body = null)
        {
            ClientResponse<T> response = await PostResponseAsync<T>(resource, body);
            return GetDataOrDefault(response);
        }

        /// <summary>
        /// PostAsync is a wrapper around the underlying Connection's PostAsync method, which validates the status code of the response
        /// </summary>
        public async Task PostAsync(string resource, object body = null)
        {
            await PostResponseAsync<object>(resource, body);
        }

        /// <summary>
        /// PutAsync is a wrapper around the underlying Connection's PutAsync method, which validates the status code of the response
        /// </summary>
        public async Task<T> PutAsync<T>(string resource, object body = null)
        {
            ClientResponse<T> response = await PutResponseAsync<T>(resource, body);
            return GetDataOrDefault(response);
        }

        /// <summary>
        /// PutAsync is a wrapper around the underlying Connection's PutAsync method, which validates the status code of the response
        /// </summary>
        public async Task PutAsync(string resource, object body = null)
        {
            await PutResponseAsync<object>(resource, body);
        }

        /// <summary>
        /// PatchAsync is a wrapper around the underlying Connection's PatchAsync method, which validates the status code of the response
        /// </summary>
        public async Task<T> PatchAsync<T>(string resource, object body = null)
        {
            ClientResponse<T> response = await PatchResponseAsync<T>(resource, body);
            return GetDataOrDefault(response);
        }

        /// <summary>
        /// PatchAsync is a wrapper around the underlying Connection's PatchAsync method, which validates the status code of the response
        /// </summary>
        public async Task PatchAsync(string resource, object body = null)
        {
            await PatchResponseAsync<object>(resource, body);
        }

        /// <summary>
        /// DeleteAsync is a wrapper around the underlying Connection's DeleteAsync method, which validates the status code of the response
        /// </summary>
        public async Task<T> DeleteAsync<T>(string resource, object body = null)
        {
            ClientResponse<T> response = await DeleteResponseAsync<T>(resource, body);
            return GetDataOrDefault(response);
        }

        /// <summary>
        /// DeleteAsync is a wrapper around the underlying Connection's DeleteAsync method, which validates the status code of the response
        /// </summary>
        public async Task DeleteAsync(string resource, object body = null)
        {
            await DeleteResponseAsync<object>(resource, body);
        }

        protected virtual async Task<ClientResponse<T>> GetResponseAsync<T>(string resource, ClientRequestParameters parameters = null)
        {
            ClientResponse<T> response = await this.Connection.GetAsync<T>(resource, parameters);
            response.Validate();

            return response;
        }

        protected virtual async Task<ClientResponse<T>> PostResponseAsync<T>(string resource, object body = null)
        {
            ClientResponse<T> response = await this.Connection.PostAsync<T>(resource, body);
            response.Validate();

            return response;
        }

        protected virtual async Task<ClientResponse<T>> PutResponseAsync<T>(string resource, object body = null)
        {
            ClientResponse<T> response = await this.Connection.PutAsync<T>(resource, body);
            response.Validate();

            return response;
        }

        protected virtual async Task<ClientResponse<T>> PatchResponseAsync<T>(string resource, object body = null)
        {
            ClientResponse<T> response = await this.Connection.PatchAsync<T>(resource, body);
            response.Validate();

            return response;
        }

        protected virtual async Task<ClientResponse<T>> DeleteResponseAsync<T>(string resource, object body = null)
        {
            ClientResponse<T> response = await this.Connection.DeleteAsync<T>(resource, body);
            response.Validate();

            return response;
        }

        protected virtual T GetDataOrDefault<T>(ClientResponse<T> response)
        {
            return (response.Body != null) ? response.Body.Data : default(T);
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
