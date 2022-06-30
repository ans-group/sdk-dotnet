using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ANS.API.Client.Response;

namespace ANS.API.Client.Core.Tests.Response
{
    [TestClass]
    public class ClientResponseErrorTests
    {
        [TestMethod]
        public void ToString_ExpectedString()
        {
            ClientResponseError error = new ClientResponseError()
            {
                Detail = "testdetail",
                Source = "testsource",
                Status = 500,
                Title = "testtitle"
            };

            string msg = error.ToString();

            Assert.AreEqual("Title=[testtitle] Detail=[testdetail] Status=[500] Source=[testsource]", msg);
        }
    }
}
