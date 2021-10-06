using System.Collections.Generic;
using AppMetadataAPIServer.Query;
using AppMetadataAPIServer.Storage;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AppMetadataAPIServer.UnitTest.Query
{
    [TestClass]
    public class QueryContextBuilderTest
    {
        private QueryContextBuilder queryContextBuilder;

        private Mock<ILogger<QueryContextBuilder>> mockLogger = new();

        [TestInitialize]
        public void TestInitialize()
        {
            this.queryContextBuilder = new QueryContextBuilder(mockLogger.Object);
        }
        
        [TestMethod]
        public void ShouldCreateCorrectQueryContext()
        {
            QueryParameters inputParameter = new QueryParameters
            {
                Title = "app",
                Version = "1.0",
                Name = "    Kai",
                Email = "",
                Company = "Cobrakai  ",
            };

            QueryContext actualResult = this.queryContextBuilder.BuildQueryContext(inputParameter);

            QueryContext expectedResult = new QueryContext
            {
                ANDClause = new List<QueryTerm>
                {
                    new QueryTerm
                    {
                        PropertyName = SearchableProperties.Title, PropertyValue = "app", Comparator = Comparator.Equal
                    },
                    new QueryTerm
                    {
                        PropertyName = SearchableProperties.Version, PropertyValue = "1.0",
                        Comparator = Comparator.Equal
                    },
                    new QueryTerm
                    {
                        PropertyName = SearchableProperties.MaintainerName, PropertyValue = "Kai",
                        Comparator = Comparator.Equal
                    },
                    new QueryTerm
                    {
                        PropertyName = SearchableProperties.Company, PropertyValue = "Cobrakai",
                        Comparator = Comparator.Equal
                    },
                }
            };

            actualResult.Should().BeEquivalentTo(expectedResult);


        }
    }
}