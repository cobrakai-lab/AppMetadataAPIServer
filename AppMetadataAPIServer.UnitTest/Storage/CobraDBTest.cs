using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppMetadataAPIServer.Exceptions;
using AppMetadataAPIServer.Models;
using AppMetadataAPIServer.Query;
using AppMetadataAPIServer.Storage;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static Cobrakai.GWTUnit.GWTUnit;

namespace AppMetadataAPIServer.UnitTest.Storage
{
    [TestClass]
    public class CobraDBTest: InputDataFixture
    {
        private CobraDB cobraDB;

        private Mock<ICobraSearch<ApplicationMetadataKey, ApplicationMetadata>> mockSearchEngine = new();
        private Mock<ILogger<CobraDB>> mockLogger = new();

        [TestInitialize]
        public void TestInitialize()
        {
            this.cobraDB = new CobraDB(mockSearchEngine.Object, mockLogger.Object);
            mockSearchEngine.Reset();
        }

        [TestMethod]
        public void ShouldBeAbleToCreateNewEntry()
        {
            Given(() =>
            {
                ApplicationMetadata appMetadata = ValidApplicationMetadata[0];
                return appMetadata;
            }).When((appMetadata) =>
            {
                this.cobraDB.Create(appMetadata);
            }).Then((appMetadata) =>
            {
                ApplicationMetadataKey expectedKey = new ApplicationMetadataKey
                {
                    Title = appMetadata.Title,
                    Verison = appMetadata.Version
                };
                this.cobraDB.FindBulk(new HashSet<ApplicationMetadataKey>() {  expectedKey })
                    .Should().BeEquivalentTo(new List<ApplicationMetadata>() { appMetadata });
                mockSearchEngine.Verify(_=>_.Index(appMetadata), Times.Once);
            });
        }
        
        [TestMethod]
        public void ShouldRejectDuplicateRecords()
        {
            Given(() =>
            {
                ApplicationMetadata appMetadata = ValidApplicationMetadata[0];
                this.cobraDB.Create(appMetadata);
                return appMetadata;
            }).When((appMetadata) =>
            {
                bool isCorrectExceptionThrown = false;
                try
                {
                    this.cobraDB.Create(appMetadata);
                }
                catch (DuplicateRecordException dre)
                {
                    isCorrectExceptionThrown = true;
                }
                return isCorrectExceptionThrown;
            }).Then(( isCorrectExceptionThrown) =>
            {
                isCorrectExceptionThrown.Should().BeTrue();
            });
            
        }
        
        [TestMethod]
        public void ShouldBeThreadSafe()
        {
            int failureCount = 0;
            ApplicationMetadata successEntry = null;
            int concurrency = 1000;

            Given(() =>
            {
                IList<Task> tasks = new List<Task>();
                ManualResetEvent mre = new ManualResetEvent(false);

                for (int i = 0; i < concurrency; i++)
                {
                    tasks.Add(Task.Run(() =>
                    {
                        try
                        {
                            mre.WaitOne();
                            var input = ValidApplicationMetadata[0];
                            input.Description = new Random().Next().ToString();
                            this.cobraDB.Create(input);
                            successEntry = input;
                        }
                        catch (DuplicateRecordException dre)
                        {
                            Interlocked.Increment(ref failureCount);
                        }
                    }));
                }

                return (mre, tasks );
            }).When(ctx =>
            {
                ctx.mre.Set();
                Task.WaitAll(ctx.tasks.ToArray(), 2000);    
            }).Then((_) =>
            {
                failureCount.Should().Be(concurrency-1);
                cobraDB.FindBulk(new HashSet<ApplicationMetadataKey>() { successEntry.CreateKey() })
                    .Should().BeEquivalentTo(new List<ApplicationMetadata> { successEntry });
                this.mockSearchEngine.Verify(_=>_.Index(successEntry), Times.Once); 
            });
        }

        [TestMethod]
        public void ShouldBeAbleToSearch()
        {
            Given(() =>
            {
                QueryContext queryContext = new QueryContext
                {
                    ANDClause = new List<QueryTerm>
                    {
                        new QueryTerm
                        {
                            PropertyName = SearchableProperties.Title,
                            PropertyValue = ValidApplicationMetadata[0].Title,
                            Comparator = Comparator.Equal
                        }
                    }
                };

                var mockReturnedKeysFromSearch = new HashSet<ApplicationMetadataKey>()
                {
                    ValidApplicationMetadata[0].CreateKey(),
                    ValidApplicationMetadata[1].CreateKey(),
                    ValidApplicationMetadata[2].CreateKey()
                };

                mockSearchEngine.Setup(_ => _.Search(queryContext))
                    .Returns(mockReturnedKeysFromSearch);
                this.cobraDB.Create(ValidApplicationMetadata[0]);
                this.cobraDB.Create(ValidApplicationMetadata[1]);
                this.cobraDB.Create(ValidApplicationMetadata[2]);

                return queryContext;

            }).When(queryContext =>
            {
                QueryResult actualResult = this.cobraDB.Query(queryContext);
                return actualResult;

            }).Then(actualResult =>
            {
                QueryResult expectedResult = new QueryResult
                {
                    IsSuccess = true,
                    resultData = new List<ApplicationMetadata>()
                    {
                        ValidApplicationMetadata[0],
                        ValidApplicationMetadata[1],
                        ValidApplicationMetadata[2]
                    }
                };

                actualResult.Should().BeEquivalentTo(expectedResult);
            }); 
        }
    }
}