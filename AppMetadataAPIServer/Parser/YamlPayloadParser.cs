using AppMetadataAPIServer.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AppMetadataAPIServer.Parser
{
    public class YamlPayloadParser: IPayloadParser
    {
        private readonly IDeserializer deserializer = new DeserializerBuilder()
            .WithNamingConvention(LowerCaseNamingConvention.Instance)
            .Build();
        
        public ApplicationMetadata Parse(string payload)
        {
            return this.deserializer.Deserialize<ApplicationMetadata>(payload);
        }
    }
}