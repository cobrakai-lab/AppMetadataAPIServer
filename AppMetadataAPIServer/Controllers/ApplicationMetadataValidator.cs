using System;
using AppMetadataAPIServer.Models;
using Microsoft.Extensions.Logging;

namespace AppMetadataAPIServer.Controllers
{
    public class ApplicationMetadataValidator : IPayloadValidator<ApplicationMetadata>
    {
        private readonly ILogger logger;

        public ApplicationMetadataValidator(ILogger<ApplicationMetadataValidator> logger)
        {
            this.logger = logger;
        }
        public ApplicationMetadata Validate(ApplicationMetadata payload)
        {
            //TODO implement
            logger.LogInformation("Payload validated successfully");
            return payload;
        }
    }
}