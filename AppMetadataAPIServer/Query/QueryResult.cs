using System.Collections.Generic;
using AppMetadataAPIServer.Models;

namespace AppMetadataAPIServer.Query
{
    public class QueryResult
    {
        public bool IsSuccess { get; set; }
        public IList<ApplicationMetadata> resultData
        {
            get;
            set;
        }
    }
}