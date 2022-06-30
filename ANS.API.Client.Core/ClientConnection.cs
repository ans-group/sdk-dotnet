using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using ANS.API.Client.Models;
using ANS.API.Client.Request;
using ANS.API.Client.Response;

namespace ANS.API.Client
{
    public class ClientConnection : IConnection
    {
        private bool _disposed = false;
        const string APIURI = "api.ukfast.io";
        const string APISCHEME = "https";
        const string APIAGENT = "ans-sdk-dotnet";
        const int VERSIONMAJOR = 1;

        public HttpClient Client { get; set; }
        private ANSCredentials Credentials { get; set; }

        /// <summary>
        /// Constructs a new ANS Client instance using provided API key
        /// </summary>
        /// <param name="apiKey">Application API key</param>
        public ClientConnection(string apiKey)
        {
            this.Client = GetDefaultClient();
            this.Credentials = new ANSAPIKeyCredentials(apiKey);
        }

        /// <summary>
        /// Constructs a new ANS Client instance using provided ANSCredentials instance
        /// </summary>
        /// <param name="credentials">Instance of ANSCredentials object</param>
        public ClientConnection(ANSCredentials credentials)
        {
            this.Client = GetDefaultClient();
            this.Credentials = credentials;
        }

        protected virtual HttpClient GetDefaultClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{APISCHEME}://{APIURI}");
            client.DefaultRequestHeaders.UserAgent.ParseAdd($"{APIAGENT}/{VERSIONMAJOR}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        /// <summary>
        /// Invokes a Get request against provided relative resource URI with provided parameters
        /// </summary>
        /// <param name="resource">Relative URI for resource</param>
        /// <param name="parameters">Optional request parameters</param>
        public async Task<ClientResponse<T>> GetAsync<T>(string resource, ClientRequestParameters parameters = null)
        {
            return await InvokeAsync<T>(new ClientRequest()
            {
                Resource = resource,
                Method = ClientRequestMethod.GET,
                Parameters = parameters
            });
        }

        /// <summary>
        /// Invokes a Post request against provided relative resource URI with provided body
        /// </summary>
        /// <param name="resource">Relative URI for resource</param>
        /// <param name="body">Optional request body</param>
        public async Task<ClientResponse<T>> PostAsync<T>(string resource, object body = null)
        {
            return await InvokeAsync<T>(resource, ClientRequestMethod.POST, body);
        }

        /// <summary>
        /// Invokes a Post request against provided relative resource URI with provided body
        /// </summary>
        /// <param name="resource">Relative URI for resource</param>
        /// <param name="body">Optional request body</param>
        public async Task<ClientResponse<T>> PutAsync<T>(string resource, object body = null)
        {
            return await InvokeAsync<T>(resource, ClientRequestMethod.PUT, body);
        }

        /// <summary>
        /// Invokes a Patch request against provided relative resource URI with provided body
        /// </summary>
        /// <param name="resource">Relative URI for resource</param>
        /// <param name="body">Optional request body</param>
        public async Task<ClientResponse<T>> PatchAsync<T>(string resource, object body = null)
        {
            return await InvokeAsync<T>(resource, ClientRequestMethod.PATCH, body);
        }

        /// <summary>
        /// Invokes a Delete request against provided relative resource URI with provided body
        /// </summary>
        /// <param name="resource">Relative URI for resource</param>
        /// <param name="body">Optional request body</param>
        public async Task<ClientResponse<T>> DeleteAsync<T>(string resource, object body = null)
        {
            return await InvokeAsync<T>(resource, ClientRequestMethod.DELETE, body);
        }

        /// <summary>
        /// Invokes a service request asyncronously
        /// </summary>
        /// <param name="request">Instance of ClientRequest object</param>
        public async Task<ClientResponse<T>> InvokeAsync<T>(string resource, ClientRequestMethod method, object body = null)
        {
            return await InvokeAsync<T>(new ClientRequest()
            {
                Resource = resource,
                Method = method,
                Body = body
            });
        }

        /// <summary>
        /// Invokes a service request asyncronously
        /// </summary>
        /// <param name="request">Instance of ClientRequest object</param>
        public async Task<ClientResponse<T>> InvokeAsync<T>(ClientRequest request)
        {
            HttpRequestMessage requestMessage = GetRequestMessage(request);

            if (request.Method != ClientRequestMethod.GET && request.Body != null)
            {
                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(request.Body);
                StringContent stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                requestMessage.Content = stringContent;
            }

            HttpResponseMessage response = await this.Client.SendAsync(requestMessage);
            string responseBody = await response.Content.ReadAsStringAsync();

            ClientResponseBody<T> body = null;
            try
            {
                body = Newtonsoft.Json.JsonConvert.DeserializeObject<ClientResponseBody<T>>(responseBody, new Newtonsoft.Json.JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });
            }
            catch (System.Exception ex)
            {
                throw new Exception.ANSClientRequestException((int)response.StatusCode, responseBody, ex);
            }

            return new ClientResponse<T>()
            {
                StatusCode = (int)response.StatusCode,
                Body = body,
                Headers = response.Headers.ToDictionary(x => x.Key, x => x.Value)
            };
        }

        protected virtual HttpRequestMessage GetRequestMessage(ClientRequest request)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(new HttpMethod(request.Method.ToString()), ComposeUri(request));
            foreach (KeyValuePair<string, IEnumerable<string>> header in this.Credentials.GetAuthHeaders())
            {
                requestMessage.Headers.Add(header.Key, header.Value);
            }

            return requestMessage;
        }

        /// <summary>
        /// Composes a relative URI from given ClientRequest instance
        /// </summary>
        /// <param name="request">Instance of ClientRequest object</param>
        /// <returns></returns>
        protected virtual string ComposeUri(ClientRequest request)
        {
            request.HydratePaginationQuery();
            request.HydrateSortingQuery();
            request.HydrateFilteringQuery();

            string queryString = null;

            if (request.Query != null && request.Query.Count > 0)
            {
                List<string> queryStringList = new List<string>();
                foreach (KeyValuePair<string, string> query in request.Query)
                {
                    if (query.Value != null)
                    {
                        queryStringList.Add($"{query.Key}={query.Value}");
                    }
                    else
                    {
                        queryStringList.Add(query.Key);
                    }
                }

                queryString = "?" + string.Join("&", queryStringList);
            }

            return request.Resource.TrimStart('/') + queryString;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (this.Client != null)
                {
                    this.Client.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
