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
    public interface IConnection : IDisposable
    {
        Task<ClientResponse<T>> GetAsync<T>(string resource, ClientRequestParameters parameters = null);
        Task<ClientResponse<T>> PostAsync<T>(string resource, object body = null);
        Task<ClientResponse<T>> PutAsync<T>(string resource, object body = null);
        Task<ClientResponse<T>> PatchAsync<T>(string resource, object body = null);
        Task<ClientResponse<T>> DeleteAsync<T>(string resource, object body = null);
        Task<ClientResponse<T>> InvokeAsync<T>(ClientRequest request);
    }
}
