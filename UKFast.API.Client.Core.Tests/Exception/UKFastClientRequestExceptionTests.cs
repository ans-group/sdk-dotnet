using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using UKFast.API.Client.Exception;
using UKFast.API.Client.Request;
using UKFast.API.Client.Response;

namespace UKFast.API.Client.Core.Tests.Exception
{
    [TestClass]
    public class UKFastClientRequestExceptionTests
    {
        [TestMethod]
        public void UKFastClientRequestException_EnumerableErrors_ExpectedMessage()
        {
            UKFastClientRequestException ex = new UKFastClientRequestException(500, new List<ClientResponseError>()
            {
                new ClientResponseError() { Detail = "testdetail1" },
                new ClientResponseError() { Detail = "testdetail2" }
            });

            Assert.AreEqual("Request failed with status code [500]: Title=[] Detail=[testdetail1] Status=[0] Source=[]; Title=[] Detail=[testdetail2] Status=[0] Source=[]", ex.Message);
        }

        [TestMethod]
        public void UKFastClientRequestException_NullErrors_ExpectedMessage()
        {
            UKFastClientRequestException ex = new UKFastClientRequestException(500, default(List<ClientResponseError>));

            Assert.AreEqual("Request failed with status code [500]: ", ex.Message);
        }

        [TestMethod]
        public void UKFastClientRequestException_SetsProperties()
        {
            var innerEx = new System.Exception("test");
            UKFastClientRequestException ex = new UKFastClientRequestException(500, new List<ClientResponseError>() { new ClientResponseError() }, innerEx);

            Assert.AreEqual(500, ex.StatusCode);
            Assert.AreEqual(1, ex.Errors.Count());
            Assert.AreEqual(innerEx, ex.InnerException);
        }
    }
}
