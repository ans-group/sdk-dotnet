using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ANS.API.Client.Core.Tests
{
    [TestClass]
    public class ANSAPIKeyCredentialsTests
    {
        [TestMethod]
        public void GetAuthHeaders_ShouldHaveExpectedHeaders()
        {
            ANSAPIKeyCredentials creds = new ANSAPIKeyCredentials("testkey");

            Dictionary<string, IEnumerable<string>> headers = creds.GetAuthHeaders();

            Assert.IsTrue(headers.ContainsKey("Authorization"));
            Assert.AreEqual("testkey", headers["Authorization"].First());
        }
    }
}
