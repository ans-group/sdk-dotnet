using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UKFast.API.Client.Models;
using UKFast.API.Client.Request;
using UKFast.API.Client.Response;

namespace UKFast.API.Client.Core.Tests.Models
{
    [TestClass]
    public class PaginatedTests
    {
        [TestMethod]
        public async Task Last_LastPage_ReturnsNull()
        {
            ClientResponse<IList<ModelBase>> response = UKFastClientTests.GetListResponse(new List<ModelBase>(), 3);
            response.Body.Metadata.Pagination.CurrentPage = 3;

            Paginated<ModelBase> paginated = new Paginated<ModelBase>(null, null, null, response);

            Paginated<ModelBase> next = await paginated.Next();

            Assert.IsNull(next);
        }

        [TestMethod]
        public async Task Previous_FirstPage_ReturnsNull()
        {
            ClientResponse<IList<ModelBase>> response = UKFastClientTests.GetListResponse(new List<ModelBase>(), 3);
            response.Body.Metadata.Pagination.CurrentPage = 1;

            Paginated<ModelBase> paginated = new Paginated<ModelBase>(null, null, null, response);

            Paginated<ModelBase> previous = await paginated.Previous();

            Assert.IsNull(previous);
        }

        [TestMethod]
        public async Task Previous_NotFirstPage_ReturnsPreviousPage()
        {
            ClientResponse<IList<ModelBase>> paginatedResponse = UKFastClientTests.GetListResponse(new List<ModelBase>(), 3);
            paginatedResponse.Body.Metadata.Pagination.CurrentPage = 2;

            ClientResponse<IList<ModelBase>> mockResponse = UKFastClientTests.GetListResponse(new List<ModelBase>(), 3);
            mockResponse.Body.Metadata.Pagination.CurrentPage = 1;

            IUKFastClient client = Substitute.For<IUKFastClient>();
            client.GetPaginatedAsync<ModelBase>("testresource", Arg.Any<ClientRequestParameters>()).Returns(Task.Run(() => new Paginated<ModelBase>(client, "testresource", new ClientRequestParameters() { Pagination = new ClientRequestPagination() }, mockResponse)));

            Paginated<ModelBase> paginated = new Paginated<ModelBase>(client, "testresource", new ClientRequestParameters() { Pagination = new ClientRequestPagination() }, paginatedResponse);
            Paginated<ModelBase> previous = await paginated.Previous();

            Assert.AreEqual(1, previous.CurrentPage);
        }

        [TestMethod]
        public async Task First_ReturnsFirstPage()
        {
            ClientResponse<IList<ModelBase>> paginatedResponse = UKFastClientTests.GetListResponse(new List<ModelBase>(), 3);
            paginatedResponse.Body.Metadata.Pagination.CurrentPage = 4;

            ClientResponse<IList<ModelBase>> mockResponse = UKFastClientTests.GetListResponse(new List<ModelBase>(), 3);
            mockResponse.Body.Metadata.Pagination.CurrentPage = 1;

            IUKFastClient client = Substitute.For<IUKFastClient>();
            client.GetPaginatedAsync<ModelBase>("testresource", Arg.Is<ClientRequestParameters>(x => x.Pagination.Page == 1)).Returns(Task.Run(() => new Paginated<ModelBase>(client, "testresource", new ClientRequestParameters() { Pagination = new ClientRequestPagination() }, mockResponse)));

            Paginated<ModelBase> paginated = new Paginated<ModelBase>(client, "testresource", new ClientRequestParameters() { Pagination = new ClientRequestPagination() }, paginatedResponse);
            Paginated<ModelBase> first = await paginated.First();

            Assert.AreEqual(1, first.CurrentPage);
        }

        [TestMethod]
        public async Task Last_ReturnsLastPage()
        {
            ClientResponse<IList<ModelBase>> paginatedResponse = UKFastClientTests.GetListResponse(new List<ModelBase>(), 10);
            paginatedResponse.Body.Metadata.Pagination.CurrentPage = 5;

            ClientResponse<IList<ModelBase>> mockResponse = UKFastClientTests.GetListResponse(new List<ModelBase>(), 10);
            mockResponse.Body.Metadata.Pagination.CurrentPage = 10;

            IUKFastClient client = Substitute.For<IUKFastClient>();
            client.GetPaginatedAsync<ModelBase>("testresource", Arg.Is<ClientRequestParameters>(x => x.Pagination.Page == 10)).Returns(Task.Run(() => new Paginated<ModelBase>(client, "testresource", new ClientRequestParameters() { Pagination = new ClientRequestPagination() }, mockResponse)));

            Paginated<ModelBase> paginated = new Paginated<ModelBase>(client, "testresource", new ClientRequestParameters() { Pagination = new ClientRequestPagination() }, paginatedResponse);
            Paginated<ModelBase> last = await paginated.Last();

            Assert.AreEqual(10, last.CurrentPage);
        }
    }
}
