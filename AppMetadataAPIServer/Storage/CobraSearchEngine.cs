using System.Collections.Generic;
using System.Linq;
using AppMetadataAPIServer.Models;
using AppMetadataAPIServer.Query;
using AppMetadataAPIServer.Utils;

namespace AppMetadataAPIServer.Storage
{
    public class CobraSearchEngine : ICobraSearch<ApplicationMetadataKey, ApplicationMetadata>
    {
        private readonly IDictionary<SearchableProperties, IDictionary<string, ISet<ApplicationMetadataKey>>>
            invertedIndex =
                new Dictionary<SearchableProperties, IDictionary<string, ISet<ApplicationMetadataKey>>>();

        public ISet<ApplicationMetadataKey> Search(QueryContext queryContext)
        {
            HashSet<ApplicationMetadataKey> keys = queryContext.AND.SelectMany(GetKeysByQueryTerm).ToHashSet();
            return keys;
        }
        
        private ISet<ApplicationMetadataKey> GetKeysByQueryTerm(QueryTerm term)
        {
            SearchableProperties propertyName = term.PropertyName;
            string propertyValue = term.PropertyValue;

            if (term.Comparator == Comparator.Equal)
            {
                return this.invertedIndex.GetOrDefault(propertyName, new Dictionary<string, ISet<ApplicationMetadataKey>>())
                    .GetOrCreate(propertyValue, new HashSet<ApplicationMetadataKey>());
            }
            else
            {
                //TODO not supporting other comparators now.
                return new HashSet<ApplicationMetadataKey>();
            }
        }
        
        public void Index(ApplicationMetadata metadata)
        {
            ApplicationMetadataKey key = metadata.CreateKey();
            IndexProperty(SearchableProperties.Title, metadata.Title.Trim(), key);
            IndexProperty(SearchableProperties.Version, metadata.Version.Trim(), key);
            IndexProperty(SearchableProperties.MaintainerName, metadata.Maintainers.Select(_ => _.Name.Trim()), key);
            IndexProperty(SearchableProperties.MaintainerEmail, metadata.Maintainers.Select(_ => _.Email.Trim()), key);
            IndexProperty(SearchableProperties.Company, metadata.Company.Trim(), key);
            IndexProperty(SearchableProperties.WebSite, metadata.Website.Trim(), key);
            IndexProperty(SearchableProperties.Source, metadata.Source.Trim(), key);
            IndexProperty(SearchableProperties.License, metadata.License.Trim(), key);
        }

        private void IndexProperty(SearchableProperties propertyName, string propertyValue,
            ApplicationMetadataKey metadataKey)
        {
            IDictionary<string, ISet<ApplicationMetadataKey>> indexForProperty =
                invertedIndex.GetOrCreate(propertyName, new Dictionary<string, ISet<ApplicationMetadataKey>>());
            indexForProperty.GetOrCreate(propertyValue, new HashSet<ApplicationMetadataKey>()).Add(metadataKey);
        }

        private void IndexProperty(SearchableProperties propertyName, IEnumerable<string> propertyValues,
            ApplicationMetadataKey metadataKey)
        {
            foreach (var propertyValue in propertyValues)
            {
                IndexProperty(propertyName, propertyValue, metadataKey);
            }
        }
    }
}