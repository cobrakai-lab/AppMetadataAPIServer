namespace AppMetadataAPIServer.Query
{
    public interface IQueryExecutor
    {
        QueryResult Run(QueryParameters parameters);
    }
}