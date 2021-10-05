using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text.Json.Serialization;
using AppMetadataAPIServer.Models;
using AppMetadataAPIServer.Storage;
using Microsoft.Extensions.Logging;

namespace AppMetadataAPIServer.Query
{
    public class QueryExecutor: IQueryExecutor
    {
        private readonly ICobraDB<ApplicationMetadataKey,ApplicationMetadata> cobraDB;
        private readonly ILogger logger;


        public QueryExecutor(ICobraDB<ApplicationMetadataKey,ApplicationMetadata> cobraDB,
            ILogger<QueryExecutor> logger)
        {
            this.cobraDB = cobraDB;
            this.logger = logger;
        }
        
        public QueryResult Run(QueryParameters parameters)
        {
            QueryContext queryContext = CreateQueryContext(parameters);
            QueryResult  result = this.cobraDB.Query(queryContext);

            return result;
        }

        private QueryContext CreateQueryContext(QueryParameters parameters)
        {
            logger.LogInformation($"Creating QueryContext from query parameters: {parameters}");
            SearchableProperties[] searchableProperties= Enum.GetValues<SearchableProperties>();

            IEnumerable<QueryTerm> terms = searchableProperties.Select(property =>
            {
                string propertyValue = GetPropertyValue(parameters, property);
                return CreateQueryTermFromParameter(property, propertyValue, Comparator.Equal);
            }).Where(_ => _ != null);
            
            QueryContext queryContext = new QueryContext
            {
                AND = terms.ToList()
            };

            logger.LogInformation($"Created QueryContext: {queryContext}");
            return queryContext;
        }

        private string GetPropertyValue(QueryParameters parameters, SearchableProperties property)
        {
            switch (property)
            {
                case SearchableProperties.Title:
                    return parameters.Title?.Trim() ?? "";
                case SearchableProperties.Version:
                    return parameters.Version?.Trim() ?? "";
                case SearchableProperties.Company:
                    return parameters.Company?.Trim() ?? "";
                case SearchableProperties.MaintainerName:
                    return parameters.Name?.Trim() ?? "";
                case SearchableProperties.MaintainerEmail:
                    return parameters.Email?.Trim() ?? "";
                case SearchableProperties.License:
                    return parameters.License?.Trim() ?? "";
                case SearchableProperties.Source:
                    return parameters.Source?.Trim() ?? "";
                case SearchableProperties.WebSite:
                    return parameters.Website?.Trim() ?? "";
                default:
                    return "";
            } 
        }

        private QueryTerm CreateQueryTermFromParameter(
            SearchableProperties propertyName,
            string propertyValue,
            Comparator comparator = Comparator.Equal)
        {
            if (!string.IsNullOrWhiteSpace(propertyValue.Trim())){
                return new QueryTerm
                {
                    PropertyName = propertyName,
                    PropertyValue = propertyValue,
                    Comparator = comparator
                };
            }
            else
            {
                return null;
            }
        }
    }
}