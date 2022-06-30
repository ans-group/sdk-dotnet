using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ANS.API.Client.Models;
using ANS.API.Client.Request;
using ANS.API.Client.Response;

namespace ANS.API.Client.Core.Tests.Models
{
    [TestClass]
    public class PaginatedTests
    {
        [TestMethod]
        public async Task Last_LastPage_ReturnsNull()
        {
            ClientRequestParameters parameters = new ClientRequestParameters()
            {
                Pagination = new ClientRequestPagination()
                {
                    Page = 3
                }
            };

            ClientResponse<IList<ModelBase>> response = ANSClientTests.GetListResponse(new List<ModelBase>(), 3);

            Paginated<ModelBase> paginated = new Paginated<ModelBase>(null, null, parameters, response);

            Paginated<ModelBase> next = await paginated.Next();

            Assert.IsNull(next);
        }

        [TestMethod]
        public async Task Previous_FirstPage_ReturnsNull()
        {
            ClientRequestParameters parameters = new ClientRequestParameters()
            {
                Pagination = new ClientRequestPagination()
                {
                    Page = 1
                }
            };

            ClientResponse<IList<ModelBase>> response = ANSClientTests.GetListResponse(new List<ModelBase>(), 3);

            Paginated<ModelBase> paginated = new Paginated<ModelBase>(null, null, parameters, response);

            Paginated<ModelBase> previous = await paginated.Previous();

            Assert.IsNull(previous);
        }

        [TestMethod]
        public async Task Previous_NotFirstPage_ReturnsPreviousPage()
        {
            ClientResponse<IList<ModelBase>> paginatedResponse = ANSClientTests.GetListResponse(new List<ModelBase>(), 3);
            ClientResponse<IList<ModelBase>> mockResponse = ANSClientTests.GetListResponse(new List<ModelBase>(), 3);

            IANSClient client = Substitute.For<IANSClient>();
            client.GetPaginatedAsync<ModelBase>("testresource", Arg.Any<ClientRequestParameters>()).Returns(x =>
            {
                return Task.Run(() => new Paginated<ModelBase>(client, "testresource", x.ArgAt<ClientRequestParameters>(1), mockResponse));
            });

            Paginated<ModelBase> paginated = new Paginated<ModelBase>(client, "testresource", new ClientRequestParameters()
            {
                Pagination = new ClientRequestPagination()
                {
                    Page = 3
                }
            }, paginatedResponse);

            Paginated<ModelBase> previous = await paginated.Previous();

            Assert.AreEqual(2, previous.CurrentPage);
        }

        [TestMethod]
        public async Task First_ReturnsFirstPage()
        {
            ClientResponse<IList<ModelBase>> paginatedResponse = ANSClientTests.GetListResponse(new List<ModelBase>(), 3);
            ClientResponse<IList<ModelBase>> mockResponse = ANSClientTests.GetListResponse(new List<ModelBase>(), 3);

            IANSClient client = Substitute.For<IANSClient>();
            client.GetPaginatedAsync<ModelBase>("testresource", Arg.Is<ClientRequestParameters>(x => x.Pagination.Page == 1)).Returns(x =>
            {
                return Task.Run(() => new Paginated<ModelBase>(client, "testresource", x.ArgAt<ClientRequestParameters>(1), mockResponse));
            });

            Paginated<ModelBase> paginated = new Paginated<ModelBase>(client, "testresource", new ClientRequestParameters(), paginatedResponse);
            Paginated<ModelBase> first = await paginated.First();

            Assert.AreEqual(1, first.CurrentPage);
        }

        [TestMethod]
        public async Task Last_ReturnsLastPage()
        {
            ClientResponse<IList<ModelBase>> paginatedResponse = ANSClientTests.GetListResponse(new List<ModelBase>(), 10);
            ClientResponse<IList<ModelBase>> mockResponse = ANSClientTests.GetListResponse(new List<ModelBase>(), 10);

            IANSClient client = Substitute.For<IANSClient>();
            client.GetPaginatedAsync<ModelBase>("testresource", Arg.Is<ClientRequestParameters>(x => x.Pagination.Page == 10)).Returns(x =>
            {
                return Task.Run(() => new Paginated<ModelBase>(client, "testresource", x.ArgAt<ClientRequestParameters>(1), mockResponse));
            });

            Paginated<ModelBase> paginated = new Paginated<ModelBase>(client, "testresource", new ClientRequestParameters(), paginatedResponse);
            Paginated<ModelBase> last = await paginated.Last();

            Assert.AreEqual(10, last.CurrentPage);
        }

        [TestMethod]
        public void TotalPages_ReturnsTotalPages()
        {
            ClientResponse<IList<ModelBase>> paginatedResponse = ANSClientTests.GetListResponse(new List<ModelBase>(), 10);
            paginatedResponse.Body.Metadata.Pagination.TotalPages = 99;

            Paginated<ModelBase> paginated = new Paginated<ModelBase>(null, "testresource", new ClientRequestParameters(), paginatedResponse);

            Assert.AreEqual(99, paginated.TotalPages);
        }

        [TestMethod]
        public void CurrentPage_ReturnsCurrentPage()
        {
            ClientResponse<IList<ModelBase>> paginatedResponse = ANSClientTests.GetListResponse(new List<ModelBase>(), 10);

            Paginated<ModelBase> paginated = new Paginated<ModelBase>(null, "testresource", new ClientRequestParameters()
            {
                Pagination = new ClientRequestPagination()
                {
                    Page = 3
                }
            }, paginatedResponse);

            Assert.AreEqual(3, paginated.CurrentPage);
        }

        [TestMethod]
        public void CurrentPage_LessThan1_Returns1()
        {
            ClientResponse<IList<ModelBase>> paginatedResponse = ANSClientTests.GetListResponse(new List<ModelBase>(), 10);

            Paginated<ModelBase> paginated = new Paginated<ModelBase>(null, "testresource", new ClientRequestParameters()
            {
                Pagination = new ClientRequestPagination()
                {
                    Page = -4
                }
            }, paginatedResponse);

            Assert.AreEqual(1, paginated.CurrentPage);
        }

        [TestMethod]
        public void Total_ReturnsTotalItems()
        {
            ClientResponse<IList<ModelBase>> paginatedResponse = ANSClientTests.GetListResponse(new List<ModelBase>(), 10);
            paginatedResponse.Body.Metadata.Pagination.Total = 99;

            Paginated<ModelBase> paginated = new Paginated<ModelBase>(null, "testresource", new ClientRequestParameters(), paginatedResponse);

            Assert.AreEqual(99, paginated.Total);
        }
    }
}
