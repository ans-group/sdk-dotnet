using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ANS.API.Client.Models;
using ANS.API.Client.Request;
using ANS.API.Client.Response;

namespace ANS.API.Client
{
    public interface IANSClient
    {
        IConnection Connection { get; }
        Task<IList<T>> GetAllAsync<T>(ANSClient.GetPaginatedAsyncFunc<T> func, ClientRequestParameters parameters = null) where T : ModelBase;
        Task<Paginated<T>> GetPaginatedAsync<T>(string resource, ClientRequestParameters parameters = null) where T : ModelBase;
        Task<T> GetAsync<T>(string resource, ClientRequestParameters parameters = null);
        Task<T> PostAsync<T>(string resource, object body = null);
        Task PostAsync(string resource, object body = null);
        Task<T> PutAsync<T>(string resource, object body = null);
        Task PutAsync(string resource, object body = null);
        Task<T> PatchAsync<T>(string resource, object body = null);
        Task PatchAsync(string resource, object body = null);
        Task<T> DeleteAsync<T>(string resource, object body = null);
        Task DeleteAsync(string resource, object body = null);
    }
}
