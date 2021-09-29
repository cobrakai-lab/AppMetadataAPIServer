namespace AppMetadataAPIServer.Models
{
    public class ApplicationMetadata
    {
        public string Title { get; set; }
        public string Version { get; set; }
        public Maintainer[] Maintainers { get; set; }
        public string Company { get; set; }
        public string Website { get; set; }
        public string Source { get; set; }
        public string License { get; set; }
        public string Description { get; set; }
    }

    public class Maintainer
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}