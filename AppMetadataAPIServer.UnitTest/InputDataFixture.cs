using System.Collections;
using System.Collections.Generic;
using AppMetadataAPIServer.Models;

namespace AppMetadataAPIServer.UnitTest
{
    public class InputDataFixture
    {
        public IList<ApplicationMetadata> ValidApplicationMetadata =>
            new List<ApplicationMetadata>()
            {
                new ApplicationMetadata
                {
                    Title = "app1",
                    Version = "0.0",
                    Maintainers = new[]
                    {
                        new Maintainer
                        {
                            Name = "Kai",
                            Email = "i@g.com"
                        }
                    },
                    Company = "cobrakai",
                    Website = "abc.com",
                    Source = "github.com",
                    License = "MIT",
                    Description = "test"
                },
                new ApplicationMetadata
                {
                    Title = "app1",
                    Version = "1.0",
                    Maintainers = new[]
                    {
                        new Maintainer
                        {
                            Name = "Kai",
                            Email = "i@g.com"
                        },
                        new Maintainer
                        {
                            Name = "Yo",
                            Email = "a@b.com"
                        }
                    },
                    Company = "cobrakai   ",
                    Website = "abc.com",
                    Source = "github.com",
                    License = "MIT",
                    Description = "test"
                },
                new ApplicationMetadata
                {
                    Title = "app2",
                    Version = "1.0",
                    Maintainers = new[]
                    {
                        new Maintainer
                        {
                            Name = "Kai",
                            Email = "i@g.com"
                        }
                    },
                    Company = "cobrakai",
                    Website = "abc.com",
                    Source = "github.com",
                    License = "MIT",
                    Description = "test"
                },
            };

    }
}