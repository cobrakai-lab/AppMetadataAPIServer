using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AppMetadataAPIServer.Exceptions;
using AppMetadataAPIServer.Models;
using AppMetadataAPIServer.RequestProcessors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using static AppMetadataAPIServer.Mock.MockDataProvider;

namespace AppMetadataAPIServer.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    [Consumes("text/plain")]
    public class MetadataController:ControllerBase
    {

        public MetadataController(
            IPayloadValidator<ApplicationMetadata> payloadValidator,
            MetadataRequestProcessor requestProcessor,
            IPayloadParser payloadParser,
            ILogger<MetadataController> logger
            )
        {
            this.appMetadataValidator = payloadValidator;
            this.requestProcessor = requestProcessor;
            this.payloadParser = payloadParser;
            this.logger = logger;
        }
        
        private readonly IPayloadValidator<ApplicationMetadata> appMetadataValidator;
        private readonly MetadataRequestProcessor requestProcessor;
        private readonly IPayloadParser payloadParser;
        
        private readonly ILogger logger;


        [HttpGet]
        public ActionResult<ApplicationMetadata[]> Get()
        {
            return new[] {mockMetadata1, mockMetadata2};
        }

       
        [HttpPost]
        public ActionResult<string> Create([FromBody]string input)
        {
            try
            {
                logger.LogInformation($"start processing request");
                
                ApplicationMetadata metadata = payloadParser.Parse(input);
                appMetadataValidator.Validate(metadata); 
                requestProcessor.Create(metadata);
                
                logger.LogInformation($"Successfully processed request");
                return new AcceptedResult((string) null, "OK");
            }
            catch (Exception e)
            {
                logger.LogError($"Got exception processing Create. Exception: {e}");
                switch (e)
                {
                    case SemanticErrorException:
                        return BadRequest("Payload is not correct YAML format");
                    case InvalidPayloadException ipe:
                        return BadRequest($"Invalid payload:{ipe.Message}");
                    default:
                        return BadRequest("Something wrong.");
                }
            }
        }
    }
}