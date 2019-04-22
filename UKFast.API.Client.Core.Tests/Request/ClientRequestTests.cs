using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using UKFast.API.Client.Request;

namespace UKFast.API.Client.Core.Tests.Request
{
    [TestClass]
    public class ClientRequestTests
    {
        [TestMethod]
        public void HydratePaginationQuery_WithPage_ExpectedQuery()
        {
            ClientRequestParameters parameters = new ClientRequestParameters()
            {
                Pagination = new ClientRequestPagination()
                {
                    Page = 3
                }
            };

            var request = new ClientRequest()
            {
                Parameters = parameters
            };

            request.HydratePaginationQuery();

            Assert.IsTrue(request.Query.ContainsKey("page"));
            Assert.AreEqual("3", request.Query["page"]);
        }

        [TestMethod]
        public void HydratePaginationQuery_WithPerPage_ExpectedQuery()
        {
            ClientRequestParameters parameters = new ClientRequestParameters()
            {
                Pagination = new ClientRequestPagination()
                {
                    PerPage = 3
                }
            };

            var request = new ClientRequest()
            {
                Parameters = parameters
            };

            request.HydratePaginationQuery();

            Assert.IsTrue(request.Query.ContainsKey("per_page"));
            Assert.AreEqual("3", request.Query["per_page"]);
        }

        [TestMethod]
        public void HydratePaginationQuery_NullParameters_MissingPaginationQueryExpected()
        {
            var request = new ClientRequest()
            {
                Parameters = null
            };

            request.HydratePaginationQuery();

            Assert.AreEqual(0, request.Query.Count);
        }

        [TestMethod]
        public void HydrateSortingQuery_WithoutDescending_ExpectedQuery()
        {
            ClientRequestParameters parameters = new ClientRequestParameters()
            {
                Sorting = new ClientRequestSorting()
                {
                    Property = "testproperty"
                }
            };

            var request = new ClientRequest()
            {
                Parameters = parameters
            };

            request.HydrateSortingQuery();

            Assert.IsTrue(request.Query.ContainsKey("sort"));
            Assert.AreEqual("testproperty:asc", request.Query["sort"]);
        }

        [TestMethod]
        public void HydrateSortingQuery_WithDescending_ExpectedQuery()
        {
            ClientRequestParameters parameters = new ClientRequestParameters()
            {
                Sorting = new ClientRequestSorting()
                {
                    Property = "testproperty",
                    Descending = true
                }
            };

            var request = new ClientRequest()
            {
                Parameters = parameters
            };

            request.HydrateSortingQuery();

            Assert.IsTrue(request.Query.ContainsKey("sort"));
            Assert.AreEqual("testproperty:desc", request.Query["sort"]);
        }

        [TestMethod]
        public void HydrateSortingQuery_NullParameters_MissingSortingQueryExpected()
        {
            var request = new ClientRequest()
            {
                Parameters = null
            };

            request.HydrateSortingQuery();

            Assert.AreEqual(0, request.Query.Count);
        }

        [TestMethod]
        public void HydrateSortingQuery_NullSortingParameters_MissingSortingQueryExpected()
        {
            var request = new ClientRequest()
            {
                Parameters = new ClientRequestParameters()
                {
                    Sorting = null
                }
            };

            request.HydrateSortingQuery();

            Assert.AreEqual(0, request.Query.Count);
        }

        [TestMethod]
        public void HydrateSortingQuery_EmptySortingProperty_MissingSortingQueryExpected()
        {
            var request = new ClientRequest()
            {
                Parameters = new ClientRequestParameters()
                {
                    Sorting = new ClientRequestSorting()
                    {
                        Property = ""
                    }
                }
            };

            request.HydrateSortingQuery();

            Assert.AreEqual(0, request.Query.Count);
        }

        [TestMethod]
        public void HydrateFilteringQuery_SingleFilter_ExpectedQuery()
        {
            ClientRequestParameters parameters = new ClientRequestParameters()
            {
                Filtering = new List<ClientRequestFiltering>
                {
                    new ClientRequestFiltering()
                    {
                        Property = "testproperty",
                        Operator = FilteringOperator.GT,
                        Value = new string[] { "5" }
                    }
                }
            };

            var request = new ClientRequest()
            {
                Parameters = parameters
            };

            request.HydrateFilteringQuery();

            Assert.IsTrue(request.Query.ContainsKey("testproperty:gt"));
            Assert.AreEqual("5", request.Query["testproperty:gt"]);
        }

        [TestMethod]
        public void HydrateFilteringQuery_MultipleFilters_ExpectedQuery()
        {
            ClientRequestParameters parameters = new ClientRequestParameters()
            {
                Filtering = new List<ClientRequestFiltering>
                {
                    new ClientRequestFiltering()
                    {
                        Property = "testproperty1",
                        Operator = FilteringOperator.GT,
                        Value = new string[]{ "5" }
                    },
                    new ClientRequestFiltering()
                    {
                        Property = "testproperty2",
                        Operator = FilteringOperator.EQ,
                        Value = new string[]{ "testvalue" }
                    }
                }
            };

            var request = new ClientRequest()
            {
                Parameters = parameters
            };

            request.HydrateFilteringQuery();

            Assert.IsTrue(request.Query.ContainsKey("testproperty1:gt"));
            Assert.AreEqual("5", request.Query["testproperty1:gt"]);

            Assert.IsTrue(request.Query.ContainsKey("testproperty2:eq"));
            Assert.AreEqual("testvalue", request.Query["testproperty2:eq"]);
        }

        [TestMethod]
        public void HydrateFilteringQuery_MultipleValues_ExpectedQuery()
        {
            ClientRequestParameters parameters = new ClientRequestParameters()
            {
                Filtering = new List<ClientRequestFiltering>
                {
                    new ClientRequestFiltering()
                    {
                        Property = "testproperty1",
                        Operator = FilteringOperator.IN,
                        Value = new string[] { "5", "6" }
                    }
                }
            };

            var request = new ClientRequest()
            {
                Parameters = parameters
            };

            request.HydrateFilteringQuery();

            Assert.IsTrue(request.Query.ContainsKey("testproperty1:in"));
            Assert.AreEqual("5,6", request.Query["testproperty1:in"]);
        }

        [TestMethod]
        public void HydrateFilteringQuery_NullParameters_MissingFilteringQueryExpected()
        {
            var request = new ClientRequest()
            {
                Parameters = null
            };

            request.HydrateFilteringQuery();

            Assert.AreEqual(0, request.Query.Count);
        }

        [TestMethod]
        public void HydrateFilteringQuery_NullFilteringParameters_MissingFilteringQueryExpected()
        {
            var request = new ClientRequest()
            {
                Parameters = new ClientRequestParameters()
                {
                    Filtering = null
                }
            };

            request.HydrateFilteringQuery();

            Assert.AreEqual(0, request.Query.Count);
        }

        [TestMethod]
        public void HydrateFilteringQuery_NoFilteringParameters_MissingFilteringQueryExpected()
        {
            var request = new ClientRequest()
            {
                Parameters = new ClientRequestParameters()
                {
                    Filtering = new List<ClientRequestFiltering>()
                }
            };

            request.HydrateFilteringQuery();

            Assert.AreEqual(0, request.Query.Count);
        }
    }
}
