using System;
using System.Collections.Generic;
using AppMetadataAPIServer.Exceptions;
using AppMetadataAPIServer.Models;
using AppMetadataAPIServer.Validators;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AppMetadataAPIServer.UnitTest
{
    [TestClass]
    public class ApplicationMetadataValidatorTest
    {
        private readonly ApplicationMetadataValidator theValidator = new ApplicationMetadataValidator(Mock.Of<ILogger<ApplicationMetadataValidator>>());
        
        [TestMethod]
        public void ShouldValidateAllFieldsProvided()
        {
            Should("throw if any field not given", () =>
            {
                var mockMetadata = GetValidApplicationMetadata();
                mockMetadata.Title = "";
                Assert.ThrowsException<InvalidPayloadException>(() => theValidator.Validate(mockMetadata));
                
                mockMetadata = GetValidApplicationMetadata();
                mockMetadata.Version = "  ";
                Assert.ThrowsException<InvalidPayloadException>(() => theValidator.Validate(mockMetadata));
                
                mockMetadata = GetValidApplicationMetadata();
                mockMetadata.Maintainers=new Maintainer[]{};
                Assert.ThrowsException<InvalidPayloadException>(() => theValidator.Validate(mockMetadata));

                mockMetadata = GetValidApplicationMetadata();
                mockMetadata.Company=null;
                Assert.ThrowsException<InvalidPayloadException>(() => theValidator.Validate(mockMetadata));
                
                mockMetadata = GetValidApplicationMetadata();
                mockMetadata.Website="";
                Assert.ThrowsException<InvalidPayloadException>(() => theValidator.Validate(mockMetadata));
                
                mockMetadata = GetValidApplicationMetadata();
                mockMetadata.Source="";
                Assert.ThrowsException<InvalidPayloadException>(() => theValidator.Validate(mockMetadata));

                mockMetadata = GetValidApplicationMetadata();
                mockMetadata.License="";
                Assert.ThrowsException<InvalidPayloadException>(() => theValidator.Validate(mockMetadata));

                mockMetadata = GetValidApplicationMetadata();
                mockMetadata.Description="";
                Assert.ThrowsException<InvalidPayloadException>(() => theValidator.Validate(mockMetadata));
            });
            
            Should("Not throw on valid metadata", () =>
            {
                var mockMetadata = GetValidApplicationMetadata();
                theValidator.Validate(mockMetadata);
            });
        }
        
        [TestMethod]
        public void ShouldValidateNoDuplicateMaintainerNames()
        {
            var mockMetadata = GetValidApplicationMetadata();
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
                var mockMetadata = GetValidApplicationMetadata();
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
                var mockMetadata = GetValidApplicationMetadata();
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

        private ApplicationMetadata GetValidApplicationMetadata()
        {
            return new ApplicationMetadata
            {
                Title = "app1",
                Version = "0.0",
                Maintainers = new[]{new Maintainer
                    {
                        Name = "Kai",
                        Email = "i@g.com"
                    }
                } ,
                Company = "cobrakai",
                Website = "abc.com",
                Source = "github.com",
                License = "MIT",
                Description ="test" 
            };    
        }

    }
}