using AppMetadataAPIServer.Models;

namespace AppMetadataAPIServer.Controllers
{
    public interface IPayloadParser
    {
        ApplicationMetadata Parse(string payload);
    }
}