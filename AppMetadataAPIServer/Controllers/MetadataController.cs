using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AppMetadataAPIServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppMetadataAPIServer.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    [Consumes("text/plain")]
    public class MetadataController:ControllerBase
    {
        private ApplicationMetadata metadata1 = new ApplicationMetadata
        {
            Title = "app0",
            Version = "0.0",
            Maintainers = new Maintainer[]{new Maintainer
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
        private ApplicationMetadata metadata2 = new ApplicationMetadata
        {
            Title = "app1",
            Version = "0.1",
            Maintainers = new Maintainer[]{new Maintainer
                {
                    Name = "Kai",
                    Email = "i@g.com"
                }
            } ,
            Company = "cobrakai",
            Website = "abc.com",
            Source = "github.com",
            License = "MIT",
            Description ="another test" 
        }; 
        
        
        [HttpGet]
        public ActionResult<ApplicationMetadata[]> Get()
        {
            return new[] {metadata1, metadata2};
        }

       
        [HttpPost]
        public ActionResult<string> Create([FromBody]string input)
        {
            Console.WriteLine($"start processing request:\n{input}");
            return new AcceptedResult((string) null, "OK");
        }
    }
}