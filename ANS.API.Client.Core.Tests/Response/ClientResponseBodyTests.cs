using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using ANS.API.Client.Request;
using ANS.API.Client.Response;

namespace ANS.API.Client.Core.Tests.Response
{
    [TestClass]
    public class ClientResponseBodyTests
    {
        [TestMethod]
        public void IsPaginated_NullMetadata_ExpectFalse()
        {
            ClientResponseBody <string>responseBody = new ClientResponseBody<string>()
            {
                Metadata = null
            };

            bool isPaginated = responseBody.IsPaginated();

            Assert.IsFalse(isPaginated);
        }

        [TestMethod]
        public void IsPaginated_NullPaginationData_ExpectFalse()
        {
            ClientResponseBody <string>responseBody = new ClientResponseBody<string>()
            {
                Metadata = new ClientResponseMetadata()
                {
                    Pagination = null
                }
            };

            bool isPaginated = responseBody.IsPaginated();

            Assert.IsFalse(isPaginated);
        }

        [TestMethod]
        public void IsPaginated_WithPaginationData_ExpectTrue()
        {
            ClientResponseBody<string> responseBody = new ClientResponseBody<string>()
            {
                Metadata = new ClientResponseMetadata()
                {
                    Pagination = new ClientResponseMetadataPagination()
                }
            };

            bool isPaginated = responseBody.IsPaginated();

            Assert.IsTrue(isPaginated);
        }
    }
}
