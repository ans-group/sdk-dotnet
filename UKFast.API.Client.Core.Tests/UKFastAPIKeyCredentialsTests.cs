using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace UKFast.API.Client.Core.Tests
{
    [TestClass]
    public class UKFastAPIKeyCredentialsTests
    {
        [TestMethod]
        public void GetAuthHeaders_ShouldHaveExpectedHeaders()
        {
            UKFastAPIKeyCredentials creds = new UKFastAPIKeyCredentials("testkey");

            Dictionary<string, IEnumerable<string>> headers = creds.GetAuthHeaders();

            Assert.IsTrue(headers.ContainsKey("Authorization"));
            Assert.AreEqual("testkey", headers["Authorization"].First());
        }
    }
}
