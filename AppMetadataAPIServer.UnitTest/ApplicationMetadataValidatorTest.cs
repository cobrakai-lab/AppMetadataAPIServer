using System;
using System.Collections.Generic;
using System.Linq;
using AppMetadataAPIServer.Exceptions;
using AppMetadataAPIServer.Models;
using AppMetadataAPIServer.Validators;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AppMetadataAPIServer.UnitTest
{
    [TestClass]
    public class ApplicationMetadataValidatorTest: InputDataFixture
    {
        private readonly ApplicationMetadataValidator theValidator = new ApplicationMetadataValidator(Mock.Of<ILogger<ApplicationMetadataValidator>>());
        
        [TestMethod]
        public void ShouldValidateAllFieldsProvided()
        {
            Should("throw if any field not given", () =>
            {
                var mockMetadata = ValidApplicationMetadata[0];
                mockMetadata.Title = "";
                Assert.ThrowsException<InvalidPayloadException>(() => theValidator.Validate(mockMetadata));
                
                mockMetadata = ValidApplicationMetadata[0];
                mockMetadata.Version = "  ";
                Assert.ThrowsException<InvalidPayloadException>(() => theValidator.Validate(mockMetadata));
                
                mockMetadata = ValidApplicationMetadata[0];
                mockMetadata.Maintainers=new Maintainer[]{};
                Assert.ThrowsException<InvalidPayloadException>(() => theValidator.Validate(mockMetadata));

                mockMetadata = ValidApplicationMetadata[0];
                mockMetadata.Company=null;
                Assert.ThrowsException<InvalidPayloadException>(() => theValidator.Validate(mockMetadata));
                
                mockMetadata = ValidApplicationMetadata[0];
                mockMetadata.Website="";
                Assert.ThrowsException<InvalidPayloadException>(() => theValidator.Validate(mockMetadata));
                
                mockMetadata = ValidApplicationMetadata[0];
                mockMetadata.Source="";
                Assert.ThrowsException<InvalidPayloadException>(() => theValidator.Validate(mockMetadata));

                mockMetadata = ValidApplicationMetadata[0];
                mockMetadata.License="";
                Assert.ThrowsException<InvalidPayloadException>(() => theValidator.Validate(mockMetadata));

                mockMetadata = ValidApplicationMetadata[0];
                mockMetadata.Description="";
                Assert.ThrowsException<InvalidPayloadException>(() => theValidator.Validate(mockMetadata));
            });
            
            Should("Not throw on valid metadata", () =>
            {
                var mockMetadata = ValidApplicationMetadata[0];
                theValidator.Validate(mockMetadata);
            });
        }
        
        [TestMethod]
        public void ShouldValidateNoDuplicateMaintainerNames()
        {
            var mockMetadata = ValidApplicationMetadata[0];
            mockMetadata.Maintainers = new List<Maintainer>()
            {
                new() { Name = "Sam", Email = "a@b.com" },
                new() { Name = "Sam", Email = "c@d.com" },
            };
            Assert.ThrowsException<InvalidPayloadException>(() => theValidator.Validate(mockMetadata)); 
        }
        
        [TestMethod]
        public void ShouldValidateEmails()
        {
            Should("throw on invalid emails", () =>
            {
                var mockMetadata = ValidApplicationMetadata[0];
                mockMetadata.Maintainers = new List<Maintainer>()
                {
                    new() { Name = "Sam", Email = "a@b." },
                };
                Assert.ThrowsException<InvalidPayloadException>(() => theValidator.Validate(mockMetadata)); 
            
                mockMetadata.Maintainers = new List<Maintainer>()
                {
                    new() { Name = "Sam", Email = "a@b" },
                };
                Assert.ThrowsException<InvalidPayloadException>(() => theValidator.Validate(mockMetadata)); 
            
                mockMetadata.Maintainers = new List<Maintainer>()
                {
                    new() { Name = "Sam", Email = "abc@def@xyz.com" },
                };
                Assert.ThrowsException<InvalidPayloadException>(() => theValidator.Validate(mockMetadata)); 
            });
            
            Should("pass on valid email",()=>
            {
                var mockMetadata = ValidApplicationMetadata[0];
                mockMetadata.Maintainers = new List<Maintainer>()
                {
                    new() { Name = "Sam", Email = "abc@xyz.com" },
                    new() { Name = "Tom", Email = "abc@xyz.net" },
                    new() { Name = "Jim", Email = "a@b.com" },
                    new() { Name = "Tim", Email = "abc+123@xyz.com" },
                    new() { Name = "Kim", Email = "abc-123@xyz.com" },
                    new() { Name = "Vim", Email = "abc_999@xyz.com" },
                };
                theValidator.Validate(mockMetadata);
            });
        }

        private void Should(string description, Action act)
        {
            act();
        }
    }
}