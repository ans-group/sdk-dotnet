using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ANS.API.Client.Exception;
using ANS.API.Client.Request;
using ANS.API.Client.Response;

namespace ANS.API.Client.Core.Tests.Response
{
    [TestClass]
    public class ClientResponseTests
    {
        [TestMethod]
        public void Validate_ExpectedValidStatusCode_NoException()
        {
            ClientResponse<object> response = new ClientResponse<object>()
            {
                StatusCode = 200
            };

            response.Validate(new int[] { 200 });
        }

        [TestMethod]
        public void Validate_ExpectedInvalidStatusCode_NoException()
        {
            ClientResponse<object> response = new ClientResponse<object>()
            {
                StatusCode = 500
            };

            response.Validate(new int[] { 500 });
        }

        [TestMethod]
        public void Validate_UnxpectedStatusCode_ThrowsANSClientRequestException()
        {
            ClientResponse<object> response = new ClientResponse<object>()
            {
                StatusCode = 500
            };

            ANSClientRequestException ex = Assert.ThrowsException<ANSClientRequestException>(() => response.Validate(new int[] { 200 }));
            Assert.AreEqual(500, ex.StatusCode);
        }

        [TestMethod]
        public void Validate_UnxpectedStatusCodeWithBody_ThrowsANSClientRequestException()
        {
            ClientResponse<object> response = new ClientResponse<object>()
            {
                StatusCode = 500,
                Body = new ClientResponseBody<object>()
                {
                    Errors = new List<ClientResponseError>()
                    {
                        new ClientResponseError()
                    }
                }
            };

            ANSClientRequestException ex = Assert.ThrowsException<ANSClientRequestException>(() => response.Validate(new int[] { 200 }));
            Assert.AreEqual(500, ex.StatusCode);
            Assert.AreEqual(1, ex.Errors.Count());
        }

        [TestMethod]
        public void Validate_404StatusCode_ThrowsANSClientNotFoundRequestException()
        {
            ClientResponse<object> response = new ClientResponse<object>()
            {
                StatusCode = 404
            };

            ANSClientNotFoundRequestException ex = Assert.ThrowsException<ANSClientNotFoundRequestException>(() => response.Validate(new int[] { 200 }));
            Assert.AreEqual(404, ex.StatusCode);
        }

        [TestMethod]
        public void Validate_404StatusCodeWithErrors_ThrowsANSClientNotFoundRequestException()
        {
            ClientResponse<object> response = new ClientResponse<object>()
            {
                StatusCode = 404,
                Body = new ClientResponseBody<object>()
                {
                    Errors = new List<ClientResponseError>()
                    {
                        new ClientResponseError()
                    }
                }
            };

            ANSClientNotFoundRequestException ex = Assert.ThrowsException<ANSClientNotFoundRequestException>(() => response.Validate(new int[] { 200 }));
            Assert.AreEqual(404, ex.StatusCode);
            Assert.AreEqual(1, ex.Errors.Count());
        }

        [TestMethod]
        public void Validate_InvalidStatusCode_ThrowsANSClientRequestException()
        {
            ClientResponse<object> response = new ClientResponse<object>()
            {
                StatusCode = 500
            };

            Assert.ThrowsException<ANSClientRequestException>(() => response.Validate());
        }

        [TestMethod]
        public void Validate_ValidStatusCode_NoException()
        {
            ClientResponse<object> response = new ClientResponse<object>()
            {
                StatusCode = 200
            };

            response.Validate();
        }

        [TestMethod]
        public void Validate_StatusCodeLessThan200_ThrowsANSClientRequestException()
        {
            ClientResponse<object> response = new ClientResponse<object>()
            {
                StatusCode = 199
            };

            Assert.ThrowsException<ANSClientRequestException>(() => response.Validate());
        }

        [TestMethod]
        public void Validate_StatusCodeGreaterThan299_ThrowsANSClientRequestException()
        {
            ClientResponse<object> response = new ClientResponse<object>()
            {
                StatusCode = 300
            };

            Assert.ThrowsException<ANSClientRequestException>(() => response.Validate());
        }
    }
}
