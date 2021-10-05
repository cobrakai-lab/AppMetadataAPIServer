using AppMetadataAPIServer.Storage;
using AppMetadataAPIServer.Utils;

namespace AppMetadataAPIServer.Query
{
    public class QueryTerm
    {
        public SearchableProperties PropertyName { get; set; }
        public string PropertyValue { get; set; }
        public Comparator Comparator { get; set; }

        public override string ToString()
        {
            return $"{PropertyName}{Comparator.ToSymbolString()}{PropertyValue}";
        }
    }
}