using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AppMetadataAPIServer.Exceptions;
using AppMetadataAPIServer.Models;
using AppMetadataAPIServer.Query;
using AppMetadataAPIServer.Utils;
using Microsoft.Extensions.Logging;

namespace AppMetadataAPIServer.Storage
{
    /// <summary>
    /// A fabulous in-memory database with a search engine!
    /// </summary>
    public class CobraDB : ICobraDB<ApplicationMetadataKey, ApplicationMetadata>
    {
        private readonly IDictionary<ApplicationMetadataKey, ApplicationMetadata> core =
            new Dictionary<ApplicationMetadataKey, ApplicationMetadata>();

        private readonly ICobraSearch<ApplicationMetadataKey,ApplicationMetadata> cobraSearch;
        private readonly object lockToken = new object();
        private readonly ILogger<CobraDB> logger;

        public CobraDB(ICobraSearch<ApplicationMetadataKey, ApplicationMetadata> searchEngine,
            ILogger<CobraDB> logger)
        {
            this.cobraSearch = searchEngine;
            this.logger = logger;
        }


        public void Create(ApplicationMetadata entry)
        {
            lock (lockToken)
            {
                CreateInternal(entry);
            }
        }

        public QueryResult Query(QueryContext queryContext)
        {
            logger.LogInformation($"Querying database for {queryContext}");
            
            ISet<ApplicationMetadataKey> keys = this.cobraSearch.Search(queryContext);
            IList<ApplicationMetadata> metadata = FindBulk(keys);

            return new QueryResult
            {
                IsSuccess = true,
                resultData = metadata
            };
        }
        
        private void CreateInternal(ApplicationMetadata entry)
        {
            var key = entry.CreateKey();
            if (this.core.ContainsKey(key))
            {
                throw new DuplicateRecordException();
            }
            this.core.Add(key, entry);
            this.cobraSearch.Index(entry);
        }
        
        private IList<ApplicationMetadata> FindBulk(ISet<ApplicationMetadataKey> keys)
        {
            return keys.Select(key => this.core.GetOrDefault(key, null)).Where(_ => _ != null).ToList();
        }

    }
}