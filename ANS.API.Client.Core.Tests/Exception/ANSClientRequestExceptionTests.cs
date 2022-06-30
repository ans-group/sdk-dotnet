using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using ANS.API.Client.Exception;
using ANS.API.Client.Request;
using ANS.API.Client.Response;

namespace ANS.API.Client.Core.Tests.Exception
{
    [TestClass]
    public class ANSClientRequestExceptionTests
    {
        [TestMethod]
        public void ANSClientRequestException_EnumerableErrors_ExpectedMessage()
        {
            ANSClientRequestException ex = new ANSClientRequestException(500, new List<ClientResponseError>()
            {
                new ClientResponseError() { Detail = "testdetail1" },
                new ClientResponseError() { Detail = "testdetail2" }
            });

            Assert.AreEqual("Request failed with status code [500]: Title=[] Detail=[testdetail1] Status=[0] Source=[]; Title=[] Detail=[testdetail2] Status=[0] Source=[]", ex.Message);
        }

        [TestMethod]
        public void ANSClientRequestException_NullErrors_ExpectedMessage()
        {
            ANSClientRequestException ex = new ANSClientRequestException(500, default(List<ClientResponseError>));

            Assert.AreEqual("Request failed with status code [500]: ", ex.Message);
        }

        [TestMethod]
        public void ANSClientRequestException_SetsProperties()
        {
            var innerEx = new System.Exception("test");
            ANSClientRequestException ex = new ANSClientRequestException(500, new List<ClientResponseError>() { new ClientResponseError() }, innerEx);

            Assert.AreEqual(500, ex.StatusCode);
            Assert.AreEqual(1, ex.Errors.Count());
            Assert.AreEqual(innerEx, ex.InnerException);
        }
    }
}
