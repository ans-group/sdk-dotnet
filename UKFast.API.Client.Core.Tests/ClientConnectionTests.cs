using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UKFast.API.Client.Exception;
using UKFast.API.Client.Request;

namespace UKFast.API.Client.Core.Tests
{
    [TestClass]
    public class ClientConnectionTests
    {
        [TestMethod]
        public void ComposeUri_NullQuery_ExpectedURI()
        {
            ClientRequest request = new ClientRequest()
            {
                Resource = "testresource",
                Query = null
            };

            TestClientConnection client = new TestClientConnection();

            string uri = client.ComposeUri_Exposed(request);

            Assert.AreEqual("testresource", uri);
        }

        [TestMethod]
        public void ComposeUri_EmptyQuery_ExpectedURI()
        {
            ClientRequest request = new ClientRequest()
            {
                Resource = "testresource",
                Query = null
            };

            TestClientConnection client = new TestClientConnection();

            string uri = client.ComposeUri_Exposed(request);

            Assert.AreEqual("testresource", uri);
        }

        [TestMethod]
        public void ComposeUri_WithKeyValueQuery_ExpectedURI()
        {
            ClientRequest request = new ClientRequest()
            {
                Resource = "testresource",
                Query = new Dictionary<string, string>()
                {
                    { "testkey","testvalue" }
                }
            };

            TestClientConnection client = new TestClientConnection();

            string uri = client.ComposeUri_Exposed(request);

            Assert.AreEqual("testresource?testkey=testvalue", uri);
        }

        [TestMethod]
        public void ComposeUri_WithKeyEmptyValueQuery_ExpectedURI()
        {
            ClientRequest request = new ClientRequest()
            {
                Resource = "testresource",
                Query = new Dictionary<string, string>()
                {
                    { "testkey","" }
                }
            };

            TestClientConnection client = new TestClientConnection();

            string uri = client.ComposeUri_Exposed(request);

            Assert.AreEqual("testresource?testkey=", uri);
        }

        [TestMethod]
        public void ComposeUri_WithKeyNullValueQuery_ExpectedURI()
        {
            ClientRequest request = new ClientRequest()
            {
                Resource = "testresource",
                Query = new Dictionary<string, string>()
                {
                    { "testkey", null }
                }
            };

            TestClientConnection client = new TestClientConnection();

            string uri = client.ComposeUri_Exposed(request);

            Assert.AreEqual("testresource?testkey", uri);
        }

        [TestMethod]
        public void ComposeUri_WithMultipleQuery_ExpectedURI()
        {
            ClientRequest request = new ClientRequest()
            {
                Resource = "testresource",
                Query = new Dictionary<string, string>()
                {
                    { "testkey1","testvalue1" },
                    { "testkey2","testvalue2" }
                }
            };

            TestClientConnection client = new TestClientConnection();

            string uri = client.ComposeUri_Exposed(request);

            Assert.AreEqual("testresource?testkey1=testvalue1&testkey2=testvalue2", uri);
        }

        [TestMethod]
        public async Task InvokeAsync_ExpectedMethod()
        {
            TestHttpMessageHandler handler = new TestHttpMessageHandler
            {
                RequestHandler = (HttpRequestMessage message) =>
                {
                    Assert.AreEqual("GET", message.Method.Method);
                }
            };

            ClientRequest request = new ClientRequest()
            {
                Resource = "testresource",
                Method = ClientRequestMethod.GET,
            };

            TestClientConnection connection = new TestClientConnection(handler);

            await connection.InvokeAsync<object>(request);
        }

        [TestMethod]
        public async Task InvokeAsync_ExpectedBody()
        {
            TestHttpMessageHandler handler = new TestHttpMessageHandler
            {
                RequestHandler = (HttpRequestMessage message) =>
                {
                    Assert.AreEqual("POST", message.Method.Method);
                    string body = Task.Run(() => message.Content.ReadAsStringAsync()).Result;
                    Assert.AreEqual("{\"testproperty\":\"testvalue\"}", body);
                }
            };

            ClientRequest request = new ClientRequest()
            {
                Resource = "testresource",
                Method = ClientRequestMethod.POST,
                Body = new { testproperty = "testvalue" }
            };

            TestClientConnection connection = new TestClientConnection(handler);

            await connection.InvokeAsync<object>(request);
        }

        [TestMethod]
        public async Task InvokeAsync_ExpectedResponseHeaders()
        {
            TestHttpMessageHandler handler = new TestHttpMessageHandler
            {
                RequestHandler = (HttpRequestMessage message) =>
                {
                    Assert.AreEqual("POST", message.Method.Method);
                    string body = Task.Run(() => message.Content.ReadAsStringAsync()).Result;
                    Assert.AreEqual("{\"testproperty\":\"testvalue\"}", body);
                },
                Headers = new Dictionary<string, string>()
                {
                    { "Test-Header-1", "TestValue1" }
                }
            };

            ClientRequest request = new ClientRequest()
            {
                Resource = "testresource",
                Method = ClientRequestMethod.POST,
                Body = new { testproperty = "testvalue" }
            };

            TestClientConnection connection = new TestClientConnection(handler);

            var response = await connection.InvokeAsync<object>(request);

            Assert.AreEqual(1, response.Headers.Count);
            Assert.AreEqual("TestValue1", response.Headers["Test-Header-1"].First());
        }

        [TestMethod]
        public async Task InvokeAsync_InvalidJsonResponse_ThrowsUKFastClientRequestException()
        {
            TestHttpMessageHandler handler = new TestHttpMessageHandler
            {
                Content = "invalidjson"
            };

            ClientRequest request = new ClientRequest()
            {
                Resource = "testresource",
                Method = ClientRequestMethod.GET
            };

            TestClientConnection connection = new TestClientConnection(handler);

            await Assert.ThrowsExceptionAsync<UKFastClientRequestException>(() => connection.InvokeAsync<object>(request));
        }

        [TestMethod]
        public async Task GetAsync_ExpectedRequest()
        {
            TestHttpMessageHandler handler = new TestHttpMessageHandler
            {
                RequestHandler = (HttpRequestMessage message) =>
                {
                    Assert.AreEqual("GET", message.Method.Method);
                    Assert.AreEqual("/testresource", message.RequestUri.AbsolutePath);
                }
            };

            TestClientConnection connection = new TestClientConnection(handler);

            await connection.GetAsync<object>("testresource");
        }

        [TestMethod]
        public async Task PostAsync_ExpectedRequest()
        {
            TestHttpMessageHandler handler = new TestHttpMessageHandler
            {
                RequestHandler = (HttpRequestMessage message) =>
                {
                    Assert.AreEqual("POST", message.Method.Method);
                    Assert.AreEqual("/testresource", message.RequestUri.AbsolutePath);
                }
            };

            TestClientConnection connection = new TestClientConnection(handler);

            await connection.PostAsync<object>("testresource");
        }

        [TestMethod]
        public async Task PutAsync_ExpectedRequest()
        {
            TestHttpMessageHandler handler = new TestHttpMessageHandler
            {
                RequestHandler = (HttpRequestMessage message) =>
                {
                    Assert.AreEqual("PUT", message.Method.Method);
                    Assert.AreEqual("/testresource", message.RequestUri.AbsolutePath);
                }
            };

            TestClientConnection connection = new TestClientConnection(handler);

            await connection.PutAsync<object>("testresource");
        }

        [TestMethod]
        public async Task PatchAsync_ExpectedRequest()
        {
            TestHttpMessageHandler handler = new TestHttpMessageHandler
            {
                RequestHandler = (HttpRequestMessage message) =>
                {
                    Assert.AreEqual("PATCH", message.Method.Method);
                    Assert.AreEqual("/testresource", message.RequestUri.AbsolutePath);
                }
            };

            TestClientConnection connection = new TestClientConnection(handler);

            await connection.PatchAsync<object>("testresource");
        }

        [TestMethod]
        public async Task DeleteAsync_ExpectedRequest()
        {
            TestHttpMessageHandler handler = new TestHttpMessageHandler
            {
                RequestHandler = (HttpRequestMessage message) =>
                {
                    Assert.AreEqual("DELETE", message.Method.Method);
                    Assert.AreEqual("/testresource", message.RequestUri.AbsolutePath);
                }
            };

            TestClientConnection connection = new TestClientConnection(handler);

            await connection.DeleteAsync<object>("testresource");
        }

        [TestMethod]
        public async Task InvokeAsync_ExpectedRequest()
        {
            TestHttpMessageHandler handler = new TestHttpMessageHandler
            {
                RequestHandler = (HttpRequestMessage message) =>
                {
                    Assert.AreEqual("POST", message.Method.Method);
                    Assert.AreEqual("/testresource", message.RequestUri.AbsolutePath);
                    string body = Task.Run(() => message.Content.ReadAsStringAsync()).Result;
                    Assert.AreEqual("{\"testproperty\":\"testvalue\"}", body);
                }
            };

            TestClientConnection connection = new TestClientConnection(handler);

            await connection.InvokeAsync<object>("testresource", ClientRequestMethod.POST, new { testproperty = "testvalue" });
        }

        [TestMethod]
        public void Dispose_DisposesClient()
        {
            TestClientConnection connection = new TestClientConnection();

            connection.Dispose();

            Assert.IsTrue(((TestHttpClient)connection.Client).Disposed);
        }
    }
}
