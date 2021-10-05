using System;

namespace AppMetadataAPIServer.Storage
{
    public class ApplicationMetadataKey: IEquatable<ApplicationMetadataKey>
    {
        public string Title { get; set; }
        public string Verison { get; set; }

        public bool Equals(ApplicationMetadataKey other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Title == other.Title && Verison == other.Verison;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ApplicationMetadataKey)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, Verison);
        }
    }
}