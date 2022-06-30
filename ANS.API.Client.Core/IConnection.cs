using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ANS.API.Client.Models;
using ANS.API.Client.Request;
using ANS.API.Client.Response;
using static ANS.API.Client.ClientConnection;

namespace ANS.API.Client
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
