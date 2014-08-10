using System;
using ConfigRemedy.Core.Properties;

namespace ConfigRemedy.Domain
{
    [UsedImplicitly, MeansImplicitUse]
    public class User : IEquatable<User>
    {
        [NotNull] public string Id { get; set; }
        [NotNull] public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }

        #region Equality comparison


        public bool Equals(User other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Id, other.Id) && string.Equals(Username, other.Username) &&
                   string.Equals(DisplayName, other.DisplayName) && string.Equals(Email, other.Email) &&
                   string.Equals(HashedPassword, other.HashedPassword);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Id != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Username != null ? Username.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (DisplayName != null ? DisplayName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Email != null ? Email.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (HashedPassword != null ? HashedPassword.GetHashCode() : 0);
                return hashCode;
            }
        }

        #endregion
    }
}