using System;
using System.Collections.Generic;
using AppMetadataAPIServer.Exceptions;
using AppMetadataAPIServer.Models;
using AppMetadataAPIServer.Query;
using AppMetadataAPIServer.RequestProcessors;
using AppMetadataAPIServer.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YamlDotNet.Core;

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
            ILogger<MetadataController> logger,
            IQueryExecutor queryExecutor
            )
        {
            this.appMetadataValidator = payloadValidator;
            this.requestProcessor = requestProcessor;
            this.payloadParser = payloadParser;
            this.logger = logger;
            this.queryExecutor = queryExecutor;
        }
        
        private readonly IPayloadValidator<ApplicationMetadata> appMetadataValidator;
        private readonly MetadataRequestProcessor requestProcessor;
        private readonly IPayloadParser payloadParser;
        private readonly IQueryExecutor queryExecutor;
        
        private readonly ILogger logger;


        [HttpGet]
        public ActionResult<QueryResult> Query([FromQuery] QueryParameters queryParams)
        {
            try
            {
                logger.LogInformation($"Starting processing query: {queryParams}");
                QueryResult result =  this.queryExecutor.Execute(queryParams);
                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError($"Got exception when processing Query request: {e}");
                return Problem("Something went wrong.");
            }
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
                        return BadRequest("Payload is not valid yaml format");
                    case YamlException ye:
                        return BadRequest($"Payload is malformed {ye.InnerException?.Message ?? ye.Message}");
                    case InvalidPayloadException ipe:
                        return BadRequest($"Invalid payload:{ipe.Message}");
                    default:
                        return Problem("Something wrong.");
                }
            }
        }
    }
}