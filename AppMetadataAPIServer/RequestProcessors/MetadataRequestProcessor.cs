using AppMetadataAPIServer.Models;
using AppMetadataAPIServer.Storage;

namespace AppMetadataAPIServer.RequestProcessors
{
    public class MetadataRequestProcessor
    {
        private readonly ICobraDB<ApplicationMetadata> cobraDB;

        public MetadataRequestProcessor(ICobraDB<ApplicationMetadata> cobraDb)
        {
            this.cobraDB = cobraDb;
        }
        public void Create(ApplicationMetadata metadata)
        {
            cobraDB.Create(metadata);
        }
        
    }
}