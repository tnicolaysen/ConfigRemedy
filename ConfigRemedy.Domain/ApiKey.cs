using System;
using ConfigRemedy.Core.Properties;

namespace ConfigRemedy.Domain
{
    public class ApiKey : IEquatable<ApiKey>
    {
        [NotNull] public string UserId { get; set; }
        public DateTime Created { get; set; } 
        public string Usage { get; set; } 
        public string HashedValue { get; set; }

        public bool Equals(ApiKey other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(UserId, other.UserId) && Created.Equals(other.Created) && string.Equals(Usage, other.Usage) && string.Equals(HashedValue, other.HashedValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ApiKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (UserId != null ? UserId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Created.GetHashCode();
                hashCode = (hashCode * 397) ^ (Usage != null ? Usage.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (HashedValue != null ? HashedValue.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}