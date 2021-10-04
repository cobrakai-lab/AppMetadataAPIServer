using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppMetadataAPIServer.Query
{
    public class QueryParameters
    {
        public string Title { get; set; }
        public string Version { get; set; }
        public string Name{ get; set; }
        public string Email{ get; set; }
        public string Company { get; set; }
        public string Website { get; set; }
        public string Source { get; set; }
        public string License { get; set; }

        public override string ToString()
        {
            return string.Join(", ", new List<string>()
            {
                string.IsNullOrWhiteSpace(Title) ? "" : $"Title={Title}",
                string.IsNullOrWhiteSpace(Version) ? "" : $"Version={Version}",
                string.IsNullOrWhiteSpace(Name) ? "" : $"Name={Name}",
                string.IsNullOrWhiteSpace(Email) ? "" : $"Email={Email}",
                string.IsNullOrWhiteSpace(Company) ? "" : $"Company={Company}",
                string.IsNullOrWhiteSpace(Website) ? "" : $"Website={Website}",
                string.IsNullOrWhiteSpace(Source) ? "" : $"Source={Source}",
                string.IsNullOrWhiteSpace(License) ? "" : $"License={License}"
            }.Where(_=>_!=""));
        }
    }
}