using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UKFast.API.Client.Exception;
using UKFast.API.Client.Models;
using UKFast.API.Client.Request;
using UKFast.API.Client.Response;
using NSubstitute;

namespace UKFast.API.Client.Core.Tests
{
    [TestClass]
    public class UKFastClientTests
    {
        public static ClientResponse<IList<ModelBase>> GetListResponse(List<ModelBase> models, int totalPages)
        {
            return new ClientResponse<IList<ModelBase>>()
            {
                Body = new ClientResponseBody<IList<ModelBase>>()
                {
                    Data = models,
                    Metadata = new ClientResponseMetadata()
                    {
                        Pagination = new ClientResponseMetadataPagination()
                        {
                            TotalPages = totalPages
                        }
                    }
                }
            };
        }

        public static ClientResponse<ModelBase> GetSingleResponse(ModelBase model, int totalPages)
        {
            return new ClientResponse<ModelBase>()
            {
                Body = new ClientResponseBody<ModelBase>()
                {
                    Data = model,
                    Metadata = new ClientResponseMetadata()
                    {
                        Pagination = new ClientResponseMetadataPagination()
                        {
                            TotalPages = totalPages
                        }
                    }
                }
            };
        }

        [TestMethod]
        public async Task GetAllAsync_ExpectedCalls()
        {
            ClientRequestParameters parameters = new ClientRequestParameters()
            {
                Pagination = new ClientRequestPagination()
            };

            ClientResponse<IList<ModelBase>> mockPage1Response = GetListResponse(new List<ModelBase>()
            {
                new ModelBase(),
                new ModelBase(),
            }, 3);
            mockPage1Response.Body.Metadata.Pagination.CurrentPage = 1;

            ClientResponse<IList<ModelBase>> mockPage2Response = GetListResponse(new List<ModelBase>()
            {
                new ModelBase(),
                new ModelBase(),
            }, 3);
            mockPage2Response.Body.Metadata.Pagination.CurrentPage = 2;

            ClientResponse<IList<ModelBase>> mockPage3Response = GetListResponse(new List<ModelBase>()
            {
                new ModelBase(),
            }, 3);
            mockPage3Response.Body.Metadata.Pagination.CurrentPage = 3;

            IUKFastClient mockClient = Substitute.For<IUKFastClient>();
            mockClient.GetPaginatedAsync<ModelBase>("testresource", Arg.Is<ClientRequestParameters>(x => x.Pagination.Page == 2)).Returns(Task.Run(() => new Paginated<ModelBase>(mockClient, "testresource", parameters, mockPage2Response)));
            mockClient.GetPaginatedAsync<ModelBase>("testresource", Arg.Is<ClientRequestParameters>(x => x.Pagination.Page == 3)).Returns(Task.Run(() => new Paginated<ModelBase>(mockClient, "testresource", parameters, mockPage3Response)));

            IUKFastClient client = new TestUKFastClient(null);
            IList<ModelBase> items = await client.GetAllAsync(async (ClientRequestParameters p) => new Paginated<ModelBase>(mockClient, "testresource", p, mockPage1Response), parameters);

            Assert.AreEqual(5, items.Count);
        }

        [TestMethod]
        public async Task GetAllAsync_EmptyData_ExpectedCalls()
        {
            ClientRequestParameters parameters = new ClientRequestParameters()
            {
                Pagination = new ClientRequestPagination()
            };

            ClientResponse<IList<ModelBase>> mockPage1Response = GetListResponse(new List<ModelBase>(), 3);
            mockPage1Response.Body.Metadata.Pagination.CurrentPage = 1;

            ClientResponse<IList<ModelBase>> mockPage2Response = GetListResponse(new List<ModelBase>(), 3);
            mockPage2Response.Body.Metadata.Pagination.CurrentPage = 2;

            ClientResponse<IList<ModelBase>> mockPage3Response = GetListResponse(new List<ModelBase>(), 3);
            mockPage3Response.Body.Metadata.Pagination.CurrentPage = 3;

            IUKFastClient mockClient = Substitute.For<IUKFastClient>();
            mockClient.GetPaginatedAsync<ModelBase>("testresource", Arg.Is<ClientRequestParameters>(x => x.Pagination.Page == 2)).Returns(Task.Run(() => new Paginated<ModelBase>(mockClient, "testresource", parameters, mockPage2Response)));
            mockClient.GetPaginatedAsync<ModelBase>("testresource", Arg.Is<ClientRequestParameters>(x => x.Pagination.Page == 3)).Returns(Task.Run(() => new Paginated<ModelBase>(mockClient, "testresource", parameters, mockPage3Response)));

            IUKFastClient client = new TestUKFastClient(null);
            IList<ModelBase> items = await client.GetAllAsync(async (ClientRequestParameters p) => new Paginated<ModelBase>(mockClient, "testresource", p, mockPage1Response), parameters);

            Assert.AreEqual(0, items.Count);
        }

        [TestMethod]
        public async Task GetAllAsync_NullData_ExpectedCalls()
        {
            ClientRequestParameters parameters = new ClientRequestParameters()
            {
                Pagination = new ClientRequestPagination()
            };

            ClientResponse<IList<ModelBase>> mockPage1Response = GetListResponse(null, 3);
            mockPage1Response.Body.Metadata.Pagination.CurrentPage = 1;

            ClientResponse<IList<ModelBase>> mockPage2Response = GetListResponse(null, 3);
            mockPage2Response.Body.Metadata.Pagination.CurrentPage = 2;

            ClientResponse<IList<ModelBase>> mockPage3Response = GetListResponse(null, 3);
            mockPage3Response.Body.Metadata.Pagination.CurrentPage = 3;

            IUKFastClient mockClient = Substitute.For<IUKFastClient>();
            mockClient.GetPaginatedAsync<ModelBase>("testresource", Arg.Is<ClientRequestParameters>(x => x.Pagination.Page == 2)).Returns(Task.Run(() => new Paginated<ModelBase>(mockClient, "testresource", parameters, mockPage2Response)));
            mockClient.GetPaginatedAsync<ModelBase>("testresource", Arg.Is<ClientRequestParameters>(x => x.Pagination.Page == 3)).Returns(Task.Run(() => new Paginated<ModelBase>(mockClient, "testresource", parameters, mockPage3Response)));

            IUKFastClient client = new TestUKFastClient(null);
            IList<ModelBase> items = await client.GetAllAsync(async (ClientRequestParameters p) => new Paginated<ModelBase>(mockClient, "testresource", p, mockPage1Response), parameters);

            Assert.AreEqual(0, items.Count);
        }

        [TestMethod]
        public async Task GetAllAsync_NullParameters_ExpectedConfiguredParameters()
        {
            ClientResponse<IList<ModelBase>> mockPage1Response = GetListResponse(null, 2);
            mockPage1Response.Body.Metadata.Pagination.CurrentPage = 1;

            ClientResponse<IList<ModelBase>> mockPage2Response = GetListResponse(null, 2);
            mockPage2Response.Body.Metadata.Pagination.CurrentPage = 2;

            IUKFastClient mockClient = Substitute.For<IUKFastClient>();
            mockClient.GetPaginatedAsync<ModelBase>("testresource", Arg.Is<ClientRequestParameters>(x => x.Pagination.Page == 2 && x.Pagination.PerPage == 99)).Returns(Task.Run(() => new Paginated<ModelBase>(mockClient, "testresource", null, mockPage2Response)));

            IUKFastClient client = new TestUKFastClient(null, new ClientConfig()
            {
                PaginationDefaultPerPage = 99
            });

            await client.GetAllAsync(async (ClientRequestParameters p) => new Paginated<ModelBase>(mockClient, "testresource", p, mockPage1Response), null);

            await mockClient.Received().GetPaginatedAsync<ModelBase>("testresource", Arg.Is<ClientRequestParameters>(x => x.Pagination.PerPage == 99));
        }

        [TestMethod]
        public async Task GetPaginatedResponseAsync_ExpectedResult()
        {
            var mockResponse = GetListResponse(new List<ModelBase>()
            {
                new ModelBase(),
                new ModelBase()
            }, 3);
            mockResponse.StatusCode = 200;

            IConnection connection = NSubstitute.Substitute.For<IConnection>();
            connection.GetAsync<IList<ModelBase>>("testresource", Arg.Any<ClientRequestParameters>()).Returns(Task.Run(() => mockResponse));

            TestUKFastClient client = new TestUKFastClient(connection);

            var result = await client.GetPaginatedAsync<ModelBase>("testresource");

            Assert.AreEqual(2, result.Items.Count);
            Assert.AreEqual(3, result.TotalPages);
        }

        [TestMethod]
        public async Task GetPaginatedResponseAsync_NullParameters_ExpectedConfiguredParameters()
        {
            var mockResponse = GetListResponse(new List<ModelBase>(), 3);
            mockResponse.StatusCode = 200;

            IConnection connection = NSubstitute.Substitute.For<IConnection>();
            connection.GetAsync<IList<ModelBase>>("testresource", Arg.Any<ClientRequestParameters>()).Returns(Task.Run(() => mockResponse));

            TestUKFastClient client = new TestUKFastClient(connection, new ClientConfig()
            {
                PaginationDefaultPerPage = 99
            });

            await client.GetPaginatedAsync<ModelBase>("testresource");

            await connection.Received().GetAsync<IList<ModelBase>>("testresource", Arg.Is<ClientRequestParameters>(x => x.Pagination.PerPage == 99));
        }

        [TestMethod]
        public async Task GetAsync_ExpectedData()
        {
            var mockResponse = GetSingleResponse(new ModelBase(), 3);
            mockResponse.StatusCode = 200;

            IConnection connection = NSubstitute.Substitute.For<IConnection>();
            connection.GetAsync<ModelBase>("testresource").Returns(Task.Run(() => mockResponse));

            TestUKFastClient client = new TestUKFastClient(connection);

            var result = await client.GetAsync<ModelBase>("testresource");

            Assert.AreEqual(mockResponse.Body.Data, result);
        }
    }
}
