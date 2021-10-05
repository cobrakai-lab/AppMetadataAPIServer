using System.Collections.Generic;
using AppMetadataAPIServer.Models;
using AppMetadataAPIServer.Query;

namespace AppMetadataAPIServer.Storage
{
    public interface ICobraDB<TKey, TVal>
    {
        void Create(TVal entry);

        QueryResult Query(QueryContext queryContext);
        
        IList<ApplicationMetadata> FindBulk(ISet<ApplicationMetadataKey> keys);
    }
}