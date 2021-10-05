using System;
using System.Collections.Generic;
using System.Linq;
using AppMetadataAPIServer.Storage;
using Microsoft.Extensions.Logging;

namespace AppMetadataAPIServer.Query
{
    public class QueryContextBuilder : IQueryContextBuilder
    {
        private readonly ILogger<QueryContextBuilder> logger;
        
        public QueryContextBuilder(ILogger<QueryContextBuilder> logger)
        {
            this.logger = logger;
        }
        
        public QueryContext BuildQueryContext(QueryParameters parameters)
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
                ANDClause = terms.ToList()
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