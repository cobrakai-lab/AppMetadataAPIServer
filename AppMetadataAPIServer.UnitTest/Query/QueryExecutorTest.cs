using AppMetadataAPIServer.Models;
using AppMetadataAPIServer.Query;
using AppMetadataAPIServer.Storage;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AppMetadataAPIServer.UnitTest.Query
{
    [TestClass]
    public class QueryExecutorTest
    {
        private QueryExecutor queryExecutor;

        private Mock<ICobraDB<ApplicationMetadataKey, ApplicationMetadata>> mockCobraDB = new();
        private Mock<IQueryContextBuilder> mockQueryContextBuilder = new();

        [TestInitialize]
        public void TestInitialize()
        {
            this.queryExecutor = new QueryExecutor(mockCobraDB.Object, mockQueryContextBuilder.Object);
        }
        
        [TestMethod]
        public void ShouldExecuteQueryCorrectly()
        {
            var input = new QueryParameters
            {
                Title = "app",
                Version = "1.0"
            };
            
            Mock<QueryContext> mockQueryContext = new Mock<QueryContext>();
            Mock<QueryResult> mockQueryResult = new Mock<QueryResult>();

            mockQueryContextBuilder.Setup(_ => _.BuildQueryContext(input))
                .Returns(mockQueryContext.Object);
            
            mockCobraDB.Setup(_=>_.Query(mockQueryContext.Object))
                .Returns(mockQueryResult.Object);
            
            var actualResult = this.queryExecutor.Execute(input);
            actualResult.Should().BeEquivalentTo(mockQueryResult.Object);
        }
    }
}