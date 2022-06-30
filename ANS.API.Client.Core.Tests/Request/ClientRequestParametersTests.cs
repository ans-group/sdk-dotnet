using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using ANS.API.Client.Request;

namespace ANS.API.Client.Core.Tests.Request
{
    [TestClass]
    public class ClientRequestParametersTests
    {
        [TestMethod]
        public void WithFiltering_AddsFilter()
        {
            ClientRequestParameters parameters = new ClientRequestParameters();

            parameters.WithFiltering(new ClientRequestFiltering()
            {
                Property = "testproperty",
                Operator = FilteringOperator.EQ,
                Value = new[] { "testvalue" }
            });

            Assert.AreEqual(1, parameters.Filtering.Count);
            Assert.AreEqual("testproperty", parameters.Filtering[0].Property);
        }

        [TestMethod]
        public void WithFiltering_AddsFilters()
        {
            ClientRequestParameters parameters = new ClientRequestParameters();

            parameters.WithFiltering(new ClientRequestFiltering[2]{
                new ClientRequestFiltering(){
                    Property = "testproperty1",
                    Operator = FilteringOperator.EQ,
                    Value = new[] { "testvalue1" }
                },
                new ClientRequestFiltering()
                {
                    Property = "testproperty2",
                    Operator = FilteringOperator.EQ,
                    Value = new[] { "testvalue2" }
                }
            });

            Assert.AreEqual(2, parameters.Filtering.Count);
            Assert.AreEqual("testproperty1", parameters.Filtering[0].Property);
            Assert.AreEqual("testproperty2", parameters.Filtering[1].Property);
        }

        [TestMethod]
        public void WithFiltering_Null_IgnoresFilter()
        {
            ClientRequestParameters parameters = new ClientRequestParameters();

            parameters.WithFiltering(default(ClientRequestFiltering));

            Assert.AreEqual(0, parameters.Filtering.Count);
        }

        [TestMethod]
        public void WithPagination_SetsPagination()
        {
            ClientRequestParameters parameters = new ClientRequestParameters();

            parameters.WithPagination(new ClientRequestPagination() { Page = 3 });

            Assert.AreEqual(3, parameters.Pagination.Page);
        }

        [TestMethod]
        public void WithSorting_SetsSorting()
        {
            ClientRequestParameters parameters = new ClientRequestParameters();

            parameters.WithSorting(new ClientRequestSorting() { Property = "testproperty1" });

            Assert.AreEqual("testproperty1", parameters.Sorting.Property);
        }

        [TestMethod]
        public void DeepCopy_CopiesFilters()
        {
            ClientRequestParameters parameters = new ClientRequestParameters();
            parameters.WithFiltering(new List<ClientRequestFiltering>()
            {
                new ClientRequestFiltering(),
                new ClientRequestFiltering()
            });

            ClientRequestParameters newParameters = parameters.DeepCopy();

            Assert.AreEqual(2, newParameters.Filtering.Count);
        }

        [TestMethod]
        public void DeepCopy_CopiesSorting()
        {
            ClientRequestParameters parameters = new ClientRequestParameters();
            parameters.WithSorting(new ClientRequestSorting()
            {
                Property = "testproperty"
            });

            ClientRequestParameters newParameters = parameters.DeepCopy();

            Assert.AreEqual("testproperty", newParameters.Sorting.Property);
        }

        [TestMethod]
        public void DeepCopy_CopiesPagination()
        {
            ClientRequestParameters parameters = new ClientRequestParameters();
            parameters.WithPagination(new ClientRequestPagination()
            {
                Page = 4
            });

            ClientRequestParameters newParameters = parameters.DeepCopy();

            Assert.AreEqual(4, newParameters.Pagination.Page);
        }
    }
}
