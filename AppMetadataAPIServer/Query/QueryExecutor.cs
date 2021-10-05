using System;
using System.Collections.Generic;
using System.Linq;
using AppMetadataAPIServer.Models;
using AppMetadataAPIServer.Storage;
using Microsoft.Extensions.Logging;

namespace AppMetadataAPIServer.Query
{
    public class QueryExecutor: IQueryExecutor
    {
        private readonly ICobraDB<ApplicationMetadataKey,ApplicationMetadata> cobraDB;
        private readonly IQueryContextBuilder queryContextBuilder;

        public QueryExecutor(ICobraDB<ApplicationMetadataKey,ApplicationMetadata> cobraDB,
            IQueryContextBuilder queryContextBuilder)
        {
            this.cobraDB = cobraDB;
            this.queryContextBuilder = queryContextBuilder;
        }
        public QueryResult Execute(QueryParameters parameters)
        {
            QueryContext queryContext = this.queryContextBuilder.BuildQueryContext(parameters);
            QueryResult  result = this.cobraDB.Query(queryContext);

            return result;
        }
    }
}