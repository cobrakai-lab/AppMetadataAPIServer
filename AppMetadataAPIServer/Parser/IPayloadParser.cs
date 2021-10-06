using AppMetadataAPIServer.Models;

namespace AppMetadataAPIServer.Parser
{
    public interface IPayloadParser
    {
        ApplicationMetadata Parse(string payload);
    }
}