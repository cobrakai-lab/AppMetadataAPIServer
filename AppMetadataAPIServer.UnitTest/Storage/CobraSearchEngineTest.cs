using System.Collections.Generic;
using AppMetadataAPIServer.Query;
using AppMetadataAPIServer.Storage;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppMetadataAPIServer.UnitTest.Storage
{
    [TestClass]
    public class CobraSearchEngineTest: InputDataFixture
    {
        private CobraSearchEngine cobraSearchEngine;

        [TestInitialize]
        public void TestInitialize()
        {
            this.cobraSearchEngine = new CobraSearchEngine();
            
            cobraSearchEngine.Index(ValidApplicationMetadata[0]);
            cobraSearchEngine.Index(ValidApplicationMetadata[1]);
            cobraSearchEngine.Index(ValidApplicationMetadata[2]);
        }
        
        [TestMethod]
        public void ShouldIndexMetadataCorrectly()
        {
           cobraSearchEngine.GetIndexedKeys(SearchableProperties.Title, ValidApplicationMetadata[0].Title)
               .Should().BeEquivalentTo(new HashSet<ApplicationMetadataKey>()
                   { ValidApplicationMetadata[0].CreateKey(), ValidApplicationMetadata[1].CreateKey() });
           
           cobraSearchEngine.GetIndexedKeys(SearchableProperties.Version, ValidApplicationMetadata[0].Version)
               .Should().BeEquivalentTo(new HashSet<ApplicationMetadataKey>()
                   { ValidApplicationMetadata[0].CreateKey() });
           
           cobraSearchEngine.GetIndexedKeys(SearchableProperties.MaintainerEmail, ValidApplicationMetadata[1].Maintainers[1].Email)
               .Should().BeEquivalentTo(new HashSet<ApplicationMetadataKey>()
                   { ValidApplicationMetadata[1].CreateKey()});
           
           cobraSearchEngine.GetIndexedKeys(SearchableProperties.MaintainerName, ValidApplicationMetadata[1].Maintainers[0].Name)
               .Should().BeEquivalentTo(new HashSet<ApplicationMetadataKey>()
                   { ValidApplicationMetadata[0].CreateKey(), ValidApplicationMetadata[1].CreateKey(), ValidApplicationMetadata[2].CreateKey() });

           cobraSearchEngine.GetIndexedKeys(SearchableProperties.Company, ValidApplicationMetadata[0].Company)
               .Should().BeEquivalentTo(new HashSet<ApplicationMetadataKey>()
                   { ValidApplicationMetadata[0].CreateKey(), ValidApplicationMetadata[1].CreateKey(), ValidApplicationMetadata[2].CreateKey() });
           
           cobraSearchEngine.GetIndexedKeys(SearchableProperties.WebSite, ValidApplicationMetadata[0].Website)
               .Should().BeEquivalentTo(new HashSet<ApplicationMetadataKey>()
                   { ValidApplicationMetadata[0].CreateKey(), ValidApplicationMetadata[1].CreateKey(), ValidApplicationMetadata[2].CreateKey() });
           
           cobraSearchEngine.GetIndexedKeys(SearchableProperties.Source, ValidApplicationMetadata[0].Source)
               .Should().BeEquivalentTo(new HashSet<ApplicationMetadataKey>()
                   { ValidApplicationMetadata[0].CreateKey(), ValidApplicationMetadata[1].CreateKey(), ValidApplicationMetadata[2].CreateKey() });

           cobraSearchEngine.GetIndexedKeys(SearchableProperties.License, ValidApplicationMetadata[0].License)
               .Should().BeEquivalentTo(new HashSet<ApplicationMetadataKey>()
                   { ValidApplicationMetadata[0].CreateKey(), ValidApplicationMetadata[1].CreateKey(), ValidApplicationMetadata[2].CreateKey() });
        }

        [TestMethod]
        public void ShouldSearchOnSimpleQuery()
        {
            QueryContext oneTerm = new QueryContext
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

            ISet<ApplicationMetadataKey> result = this.cobraSearchEngine.Search(oneTerm);
            result.Should().BeEquivalentTo(
                new HashSet<ApplicationMetadataKey>()
                    { ValidApplicationMetadata[0].CreateKey(), ValidApplicationMetadata[1].CreateKey() });
        }

        [TestMethod]
        public void ShouldSearchByIntersectKeys()
        {
            QueryContext twoTerms= new QueryContext
            {
                ANDClause = new List<QueryTerm>
                {
                    new QueryTerm
                    {
                        PropertyName = SearchableProperties.Title,
                        PropertyValue = ValidApplicationMetadata[0].Title,
                        Comparator = Comparator.Equal
                    },
                    new QueryTerm
                    {
                        PropertyName = SearchableProperties.Version,
                        PropertyValue = ValidApplicationMetadata[0].Version,
                        Comparator = Comparator.Equal
                    }

                }
            };

            ISet<ApplicationMetadataKey> result = this.cobraSearchEngine.Search(twoTerms);
            result.Should().BeEquivalentTo(
                new HashSet<ApplicationMetadataKey>()
                    { ValidApplicationMetadata[0].CreateKey()}); 
        }
        
        [TestMethod]
        public void ShouldReturnEmptyOnDisjointQueries()
        {
            QueryContext twoTerms= new QueryContext
            {
                ANDClause = new List<QueryTerm>
                {
                    new QueryTerm
                    {
                        PropertyName = SearchableProperties.Title,
                        PropertyValue = ValidApplicationMetadata[2].Title,
                        Comparator = Comparator.Equal
                    },
                    new QueryTerm
                    {
                        PropertyName = SearchableProperties.Version,
                        PropertyValue = ValidApplicationMetadata[0].Version,
                        Comparator = Comparator.Equal
                    }
                }
            };

            ISet<ApplicationMetadataKey> result = this.cobraSearchEngine.Search(twoTerms);
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void ShouldReturnEmptyWhenNoQueryTerms()
        {
            QueryContext emptyTerms = new QueryContext();
            ISet<ApplicationMetadataKey> result = this.cobraSearchEngine.Search(emptyTerms);
            result.Should().BeEmpty(); 
        }

    }
}