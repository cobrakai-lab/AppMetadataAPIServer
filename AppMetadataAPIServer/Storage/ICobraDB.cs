using AppMetadataAPIServer.Query;

namespace AppMetadataAPIServer.Storage
{
    public interface ICobraDB<TKey, TVal>
    {
        void Create(TVal entry);

        QueryResult Query(QueryContext queryContext);
    }
}