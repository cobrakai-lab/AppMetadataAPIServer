using AppMetadataAPIServer.Models;
using AppMetadataAPIServer.Storage;

namespace AppMetadataAPIServer.RequestProcessors
{
    public class MetadataRequestProcessor
    {
        private readonly ICobraDB<ApplicationMetadataKey,ApplicationMetadata> cobraDB;

        public MetadataRequestProcessor(ICobraDB<ApplicationMetadataKey,ApplicationMetadata> cobraDb)
        {
            this.cobraDB = cobraDb;
        }
        public void Create(ApplicationMetadata metadata)
        {
            cobraDB.Create(metadata);
        }
        
        //todo move queryExecutor in here and create new public function to handle Query.
        
    }
}