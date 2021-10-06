using System.Collections.Generic;
using System.Linq;
using AppMetadataAPIServer.Controllers;
using AppMetadataAPIServer.Exceptions;
using AppMetadataAPIServer.Models;
using Microsoft.Extensions.Logging;

namespace AppMetadataAPIServer.Validators
{
    public class ApplicationMetadataValidator : IPayloadValidator<ApplicationMetadata>
    {
        private readonly ILogger logger;

        public ApplicationMetadataValidator(ILogger<ApplicationMetadataValidator> logger)
        {
            this.logger = logger;
        }
        public void Validate(ApplicationMetadata appMetadata)
        {
            ValidateTitle(appMetadata);
            ValidateVersion(appMetadata);
            ValidateMaintainers(appMetadata);
            ValidateCompany(appMetadata);
            ValidateWebSite(appMetadata);
            ValidateSource(appMetadata);
            ValidateLicense(appMetadata);
            ValidateDescription(appMetadata);
            
            logger.LogInformation("appMetadata validated successfully");
        }

        private void ValidateTitle(ApplicationMetadata appMetadata)
        {
            if (string.IsNullOrWhiteSpace(appMetadata.Title))
            {
                throw new InvalidPayloadException("Title is required.");
            }
        }
        
        private void ValidateVersion(ApplicationMetadata appMetadata)
        {
            if (string.IsNullOrWhiteSpace(appMetadata.Version))
            {
                throw new InvalidPayloadException("Version is required.");
            }
        }
        
        private void ValidateMaintainers(ApplicationMetadata appMetadata)
        {
            if (!appMetadata.Maintainers?.Any() ?? true)
            {
                throw new InvalidPayloadException("Maintainers is required.");
            }

            foreach (Maintainer maintainer in appMetadata.Maintainers)
            {
                ValidateMaintainer(maintainer);
            }

            ValidateNoDuplicateNames(appMetadata.Maintainers);
        }

        private void ValidateNoDuplicateNames(IList<Maintainer> maintainers)
        {
            if (maintainers.Select(_ => _.Name.Trim()).Distinct().Count() != maintainers.Count)
            {
                throw new InvalidPayloadException("Maintainers should be unique");
            }
        }

        private void ValidateMaintainer(Maintainer maintainer)
        {
            ValidateMaintainerName(maintainer.Name);
            ValidateMaintainerEmail(maintainer.Email);
        }

        private void ValidateMaintainerEmail(string maintainerEmail)
        {
            if (string.IsNullOrWhiteSpace(maintainerEmail))
            {
                throw new InvalidPayloadException("Maintainer email is required");
            }

            if (!EmailFormatValidator.IsValidEmailFormat(maintainerEmail))
            {
                throw new InvalidPayloadException($"Maintainer email is invalid format: {maintainerEmail}");
            }
        }

        private void ValidateMaintainerName(string maintainerName)
        {
            if (string.IsNullOrWhiteSpace(maintainerName))
            {
                throw new InvalidPayloadException("Maintainer name is required");
            }
        }

        private void ValidateCompany(ApplicationMetadata appMetadata)
        {
            if (string.IsNullOrWhiteSpace(appMetadata.Company))
            {
                throw new InvalidPayloadException("Company is required.");
            }
        }

        private void ValidateWebSite(ApplicationMetadata appMetadata)
        {
            if (string.IsNullOrWhiteSpace(appMetadata.Website))
            {
                throw new InvalidPayloadException("Website is required.");
            }
            //todo validate correct URL format
        }

        private void ValidateSource(ApplicationMetadata appMetadata)
        {
            if (string.IsNullOrWhiteSpace(appMetadata.Source))
            {
                throw new InvalidPayloadException("Source is required.");
            }
        }

        private void ValidateLicense(ApplicationMetadata appMetadata)
        {
            if (string.IsNullOrWhiteSpace(appMetadata.License))
            {
                throw new InvalidPayloadException("License is required.");
            }
        }

        private void ValidateDescription(ApplicationMetadata appMetadata)
        {
            if (string.IsNullOrWhiteSpace(appMetadata.Description))
            {
                throw new InvalidPayloadException("Description is required.");
            }
        }
    }
}