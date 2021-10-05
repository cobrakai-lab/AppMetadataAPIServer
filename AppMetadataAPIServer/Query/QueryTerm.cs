using AppMetadataAPIServer.Storage;

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

    public enum Comparator
    {
        Equal
    }

    public static class ComparatorExtension
    {
        public static string ToSymbolString(this Comparator comparator)
        {
            switch (comparator)
            {
                case Comparator.Equal:
                    return "=";
                default:
                    return "n/a";
            }
        }
    }
}