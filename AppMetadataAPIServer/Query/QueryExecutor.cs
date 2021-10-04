using System.Collections.Generic;
using AppMetadataAPIServer.Models;
using AppMetadataAPIServer.Storage;

namespace AppMetadataAPIServer.Query
{
    public interface IQueryExecutor
    {
        QueryResult Run(QueryParameters parameters);
    }
    
    public class QueryExecutor: IQueryExecutor
    {
        private readonly ICobraDB<ApplicationMetadata> cobraDB;

        public QueryExecutor(ICobraDB<ApplicationMetadata> cobraDB)
        {
            this.cobraDB = cobraDB;
        }
        
        public QueryResult Run(QueryParameters parameters)
        {
            //todo implement
            return new QueryResult
            {
                IsSuccess = true,
                resultData = new List<ApplicationMetadata>()
                {
                    MockData.MockDataProvider.mockMetadata1,
                    MockData.MockDataProvider.mockMetadata2,
                }
            };
        }
    }
}