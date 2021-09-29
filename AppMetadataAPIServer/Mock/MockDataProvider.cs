using AppMetadataAPIServer.Models;

namespace AppMetadataAPIServer.Mock
{
    public static class MockDataProvider
    {
        public static ApplicationMetadata mockMetadata1 = new ApplicationMetadata
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
        
        public static ApplicationMetadata mockMetadata2 = new ApplicationMetadata
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
    }
}