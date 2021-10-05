namespace AppMetadataAPIServer.Query
{
    public interface IQueryExecutor
    {
        QueryResult Execute(QueryParameters parameters);
    }
}