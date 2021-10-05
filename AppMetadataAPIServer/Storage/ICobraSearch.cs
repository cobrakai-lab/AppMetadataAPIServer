using System.Collections.Generic;
using AppMetadataAPIServer.Query;

namespace AppMetadataAPIServer.Storage
{
    public interface ICobraSearch<TKey, TVal>
    {
        ISet<TKey> Search(QueryContext queryContext);
        void Index(TVal entry);
    }
}