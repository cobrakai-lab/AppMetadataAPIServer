using AppMetadataAPIServer.Query;

namespace AppMetadataAPIServer.Utils
{
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