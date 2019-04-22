using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using UKFast.API.Client.Exception;
using UKFast.API.Client.Request;
using UKFast.API.Client.Response;

namespace UKFast.API.Client.Core.Tests.Response
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
        public void Validate_UnxpectedStatusCode_ThrowsUKFastClientRequestException()
        {
            ClientResponse<object> response = new ClientResponse<object>()
            {
                StatusCode = 500
            };

            UKFastClientRequestException ex = Assert.ThrowsException<UKFastClientRequestException>(() => response.Validate(new int[] { 200 }));
            Assert.AreEqual(500, ex.StatusCode);
        }

        [TestMethod]
        public void Validate_UnxpectedStatusCodeWithBody_ThrowsUKFastClientRequestException()
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

            UKFastClientRequestException ex = Assert.ThrowsException<UKFastClientRequestException>(() => response.Validate(new int[] { 200 }));
            Assert.AreEqual(500, ex.StatusCode);
            Assert.AreEqual(1, ex.Errors.Count());
        }

        [TestMethod]
        public void Validate_404StatusCode_ThrowsUKFastClientNotFoundRequestException()
        {
            ClientResponse<object> response = new ClientResponse<object>()
            {
                StatusCode = 404
            };

            UKFastClientNotFoundRequestException ex = Assert.ThrowsException<UKFastClientNotFoundRequestException>(() => response.Validate(new int[] { 200 }));
            Assert.AreEqual(404, ex.StatusCode);
        }

        [TestMethod]
        public void Validate_404StatusCodeWithErrors_ThrowsUKFastClientNotFoundRequestException()
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

            UKFastClientNotFoundRequestException ex = Assert.ThrowsException<UKFastClientNotFoundRequestException>(() => response.Validate(new int[] { 200 }));
            Assert.AreEqual(404, ex.StatusCode);
            Assert.AreEqual(1, ex.Errors.Count());
        }

        [TestMethod]
        public void Validate_InvalidStatusCode_ThrowsUKFastClientRequestException()
        {
            ClientResponse<object> response = new ClientResponse<object>()
            {
                StatusCode = 500
            };

            Assert.ThrowsException<UKFastClientRequestException>(() => response.Validate());
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
        public void Validate_StatusCodeLessThan200_ThrowsUKFastClientRequestException()
        {
            ClientResponse<object> response = new ClientResponse<object>()
            {
                StatusCode = 199
            };

            Assert.ThrowsException<UKFastClientRequestException>(() => response.Validate());
        }

        [TestMethod]
        public void Validate_StatusCodeGreaterThan299_ThrowsUKFastClientRequestException()
        {
            ClientResponse<object> response = new ClientResponse<object>()
            {
                StatusCode = 300
            };

            Assert.ThrowsException<UKFastClientRequestException>(() => response.Validate());
        }
    }
}
