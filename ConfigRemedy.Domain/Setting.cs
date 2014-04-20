using System;

namespace ConfigRemedy.Domain
{
    public class Setting : IEquatable<Setting>
    {
        public string Key { get; set; }
        public string Value { get; set; }

        #region Equality checking 

        public bool Equals(Setting other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Key, other.Key) && string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Setting) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Key.GetHashCode()*397) ^ Value.GetHashCode();
            }
        }

        public static bool operator ==(Setting left, Setting right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Setting left, Setting right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}