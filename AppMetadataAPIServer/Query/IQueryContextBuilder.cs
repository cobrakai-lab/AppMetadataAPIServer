namespace AppMetadataAPIServer.Query
{
    public interface IQueryContextBuilder
    {
        QueryContext BuildQueryContext(QueryParameters parameters);
    }
}