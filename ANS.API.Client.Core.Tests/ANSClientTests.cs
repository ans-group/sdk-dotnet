using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ANS.API.Client.Exception;
using ANS.API.Client.Models;
using ANS.API.Client.Request;
using ANS.API.Client.Response;
using NSubstitute;

namespace ANS.API.Client.Core.Tests
{
    [TestClass]
    public class ANSClientTests
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
            ClientResponse<IList<ModelBase>> mockPage1Response = GetListResponse(new List<ModelBase>()
            {
                new ModelBase(),
                new ModelBase(),
            }, 3);

            ClientResponse<IList<ModelBase>> mockPage2Response = GetListResponse(new List<ModelBase>()
            {
                new ModelBase(),
                new ModelBase(),
            }, 3);

            ClientResponse<IList<ModelBase>> mockPage3Response = GetListResponse(new List<ModelBase>()
            {
                new ModelBase(),
            }, 3);

            IANSClient mockClient = Substitute.For<IANSClient>();
            mockClient.GetPaginatedAsync<ModelBase>("testresource", Arg.Is<ClientRequestParameters>(x => x.Pagination.Page == 2)).Returns(x =>
            {
                return Task.Run(() => new Paginated<ModelBase>(mockClient, "testresource", x.ArgAt<ClientRequestParameters>(1), mockPage2Response));
            });
            mockClient.GetPaginatedAsync<ModelBase>("testresource", Arg.Is<ClientRequestParameters>(x => x.Pagination.Page == 3)).Returns(x =>
            {
                return Task.Run(() => new Paginated<ModelBase>(mockClient, "testresource", x.ArgAt<ClientRequestParameters>(1), mockPage3Response));
            });

            IANSClient client = new TestANSClient(null);
            IList<ModelBase> items = await client.GetAllAsync(async (ClientRequestParameters p) => new Paginated<ModelBase>(mockClient, "testresource", p, mockPage1Response), new ClientRequestParameters()
            {
                Pagination = new ClientRequestPagination()
                {
                    Page = 1
                }
            });

            Assert.AreEqual(5, items.Count);
        }

        [TestMethod]
        public async Task GetAllAsync_EmptyData_ExpectedCalls()
        {
            ClientResponse<IList<ModelBase>> mockPage1Response = GetListResponse(new List<ModelBase>(), 3);
            ClientResponse<IList<ModelBase>> mockPage2Response = GetListResponse(new List<ModelBase>(), 3);
            ClientResponse<IList<ModelBase>> mockPage3Response = GetListResponse(new List<ModelBase>(), 3);

            IANSClient mockClient = Substitute.For<IANSClient>();
            mockClient.GetPaginatedAsync<ModelBase>("testresource", Arg.Is<ClientRequestParameters>(x => x.Pagination.Page == 2)).Returns(x =>
            {
                return Task.Run(() => new Paginated<ModelBase>(mockClient, "testresource", x.ArgAt<ClientRequestParameters>(1), mockPage2Response));
            });
            mockClient.GetPaginatedAsync<ModelBase>("testresource", Arg.Is<ClientRequestParameters>(x => x.Pagination.Page == 3)).Returns(x =>
            {
                return Task.Run(() => new Paginated<ModelBase>(mockClient, "testresource", x.ArgAt<ClientRequestParameters>(1), mockPage3Response));
            });

            IANSClient client = new TestANSClient(null);
            IList<ModelBase> items = await client.GetAllAsync(async (ClientRequestParameters p) => new Paginated<ModelBase>(mockClient, "testresource", p, mockPage1Response), new ClientRequestParameters()
            {
                Pagination = new ClientRequestPagination()
            });

            Assert.AreEqual(0, items.Count);
        }

        [TestMethod]
        public async Task GetAllAsync_NullData_ExpectedCalls()
        {
            ClientResponse<IList<ModelBase>> mockPage1Response = GetListResponse(null, 3);
            ClientResponse<IList<ModelBase>> mockPage2Response = GetListResponse(null, 3);
            ClientResponse<IList<ModelBase>> mockPage3Response = GetListResponse(null, 3);

            IANSClient mockClient = Substitute.For<IANSClient>();
            mockClient.GetPaginatedAsync<ModelBase>("testresource", Arg.Is<ClientRequestParameters>(x => x.Pagination.Page == 2)).Returns(x =>
            {
                return Task.Run(() => new Paginated<ModelBase>(mockClient, "testresource", x.ArgAt<ClientRequestParameters>(1), mockPage2Response));
            });
            mockClient.GetPaginatedAsync<ModelBase>("testresource", Arg.Is<ClientRequestParameters>(x => x.Pagination.Page == 3)).Returns(x =>
            {
                return Task.Run(() => new Paginated<ModelBase>(mockClient, "testresource", x.ArgAt<ClientRequestParameters>(1), mockPage3Response));
            });

            IANSClient client = new TestANSClient(null);
            IList<ModelBase> items = await client.GetAllAsync(async (ClientRequestParameters p) => new Paginated<ModelBase>(mockClient, "testresource", p, mockPage1Response), null);

            Assert.AreEqual(0, items.Count);
        }

        [TestMethod]
        public async Task GetAllAsync_NullParameters_ExpectedConfiguredParameters()
        {
            ClientResponse<IList<ModelBase>> mockPage1Response = GetListResponse(null, 2);
            ClientResponse<IList<ModelBase>> mockPage2Response = GetListResponse(null, 2);

            IANSClient mockClient = Substitute.For<IANSClient>();
            mockClient.GetPaginatedAsync<ModelBase>("testresource", Arg.Is<ClientRequestParameters>(x => x.Pagination.Page == 2 && x.Pagination.PerPage == 99)).Returns(Task.Run(() => new Paginated<ModelBase>(mockClient, "testresource", null, mockPage2Response)));

            IANSClient client = new TestANSClient(null, new ClientConfig()
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

            TestANSClient client = new TestANSClient(connection);

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

            TestANSClient client = new TestANSClient(connection, new ClientConfig()
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

            TestANSClient client = new TestANSClient(connection);

            var result = await client.GetAsync<ModelBase>("testresource");

            Assert.AreEqual(mockResponse.Body.Data, result);
        }

        [TestMethod]
        public async Task PostAsyncGeneric_ExpectedCall()
        {
            ModelBase model = new ModelBase();

            var mockResponse = GetSingleResponse(model, 0);
            mockResponse.StatusCode = 200;

            IConnection connection = NSubstitute.Substitute.For<IConnection>();
            connection.PostAsync<ModelBase>("testresource").Returns(Task.Run(() => mockResponse));

            TestANSClient client = new TestANSClient(connection);

            var result = await client.PostAsync<ModelBase>("testresource");

            Assert.AreEqual(model, result);
            await connection.Received().PostAsync<ModelBase>("testresource");
        }

        [TestMethod]
        public async Task PostAsync_ExpectedCall()
        {
            IConnection connection = NSubstitute.Substitute.For<IConnection>();
            connection.PostAsync<object>("testresource").Returns(Task.Run(() => new ClientResponse<object>()
            {
                StatusCode = 200
            }));

            TestANSClient client = new TestANSClient(connection);

            await client.PostAsync("testresource");

            await connection.Received().PostAsync<object>("testresource");
        }

        [TestMethod]
        public async Task PutAsyncGeneric_ExpectedCall()
        {
            ModelBase model = new ModelBase();

            var mockResponse = GetSingleResponse(model, 0);
            mockResponse.StatusCode = 200;

            IConnection connection = NSubstitute.Substitute.For<IConnection>();
            connection.PutAsync<ModelBase>("testresource").Returns(Task.Run(() => mockResponse));

            TestANSClient client = new TestANSClient(connection);

            var result = await client.PutAsync<ModelBase>("testresource");

            Assert.AreEqual(model, result);
            await connection.Received().PutAsync<ModelBase>("testresource");
        }

        [TestMethod]
        public async Task PutAsync_ExpectedCall()
        {
            IConnection connection = NSubstitute.Substitute.For<IConnection>();
            connection.PutAsync<object>("testresource").Returns(Task.Run(() => new ClientResponse<object>()
            {
                StatusCode = 200
            }));

            TestANSClient client = new TestANSClient(connection);

            await client.PutAsync("testresource");

            await connection.Received().PutAsync<object>("testresource");
        }

        [TestMethod]
        public async Task PatchAsyncGeneric_ExpectedCall()
        {
            ModelBase model = new ModelBase();

            var mockResponse = GetSingleResponse(model, 0);
            mockResponse.StatusCode = 200;

            IConnection connection = NSubstitute.Substitute.For<IConnection>();
            connection.PatchAsync<ModelBase>("testresource").Returns(Task.Run(() => mockResponse));

            TestANSClient client = new TestANSClient(connection);

            var result = await client.PatchAsync<ModelBase>("testresource");

            Assert.AreEqual(model, result);
            await connection.Received().PatchAsync<ModelBase>("testresource");
        }

        [TestMethod]
        public async Task PatchAsync_ExpectedCall()
        {
            IConnection connection = NSubstitute.Substitute.For<IConnection>();
            connection.PatchAsync<object>("testresource").Returns(Task.Run(() => new ClientResponse<object>()
            {
                StatusCode = 200
            }));

            TestANSClient client = new TestANSClient(connection);

            await client.PatchAsync("testresource");

            await connection.Received().PatchAsync<object>("testresource");
        }

        [TestMethod]
        public async Task DeleteAsyncGeneric_ExpectedCall()
        {
            ModelBase model = new ModelBase();

            var mockResponse = GetSingleResponse(model, 0);
            mockResponse.StatusCode = 200;

            IConnection connection = NSubstitute.Substitute.For<IConnection>();
            connection.DeleteAsync<ModelBase>("testresource").Returns(Task.Run(() => mockResponse));

            TestANSClient client = new TestANSClient(connection);

            var result = await client.DeleteAsync<ModelBase>("testresource");

            Assert.AreEqual(model, result);
            await connection.Received().DeleteAsync<ModelBase>("testresource");
        }

        [TestMethod]
        public async Task DeleteAsync_ExpectedCall()
        {
            IConnection connection = NSubstitute.Substitute.For<IConnection>();
            connection.DeleteAsync<object>("testresource").Returns(Task.Run(() => new ClientResponse<object>()
            {
                StatusCode = 200
            }));

            TestANSClient client = new TestANSClient(connection);

            await client.DeleteAsync("testresource");

            await connection.Received().DeleteAsync<object>("testresource");
        }

        [TestMethod]
        public void GetDataOrDefault_NullBody_ReturnsNull()
        {
            var response = new ClientResponse<ModelBase>()
            {
                Body = null
            };

            TestANSClient client = new TestANSClient(null);
            var result = client.GetDataOrDefault_Exposed(response);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetDataOrDefault_PopulatedBody_ReturnsBodyData()
        {
            var model = new ModelBase();
            var response = new ClientResponse<ModelBase>()
            {
                Body = new ClientResponseBody<ModelBase>()
                {
                    Data = model
                }
            };

            TestANSClient client = new TestANSClient(null);
            var result = client.GetDataOrDefault_Exposed(response);

            Assert.AreEqual(model, result);
        }
    }
}
