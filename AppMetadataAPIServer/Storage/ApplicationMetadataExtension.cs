using AppMetadataAPIServer.Models;

namespace AppMetadataAPIServer.Storage
{
    public static class ApplicationMetadataExtension
    {
        public static ApplicationMetadataKey CreateKey(this ApplicationMetadata appMetadata)
        {
            return new ApplicationMetadataKey
            {
                Title = appMetadata.Title,
                Verison = appMetadata.Version
            };
        }
    }
}