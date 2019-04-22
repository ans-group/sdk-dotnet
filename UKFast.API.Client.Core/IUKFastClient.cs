using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UKFast.API.Client.Models;
using UKFast.API.Client.Request;
using UKFast.API.Client.Response;

namespace UKFast.API.Client
{
    public interface IUKFastClient
    {
        IConnection Connection { get; }
        Task<IList<T>> GetAllAsync<T>(UKFastClient.GetPaginatedAsyncFunc<T> func, ClientRequestParameters parameters = null) where T : ModelBase;
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
