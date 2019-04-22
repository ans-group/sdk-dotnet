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
        Task<IList<T>> GetListAsync<T>(string resource, ClientRequestParameters parameters = null);
        Task<T> GetAsync<T>(string resource, ClientRequestParameters parameters = null);
    }
}
