using System;
using System.Collections.Generic;
using System.Linq;
using ConfigRemedy.Domain.Annotations;

namespace ConfigRemedy.Domain
{
    [UsedImplicitly]
    public class Application : IEquatable<Application>
    {
        public string Id { get; [UsedImplicitly] set; }
        public string Name { get; [UsedImplicitly]set; }
        public List<Setting> Settings { get; set; }

        public Application()
        {
            Settings = new List<Setting>();
        }

        public Setting GetSetting(string key)
        {
            return Settings.SingleOrDefault(KeyMatcher(key));
        }

        public void AddSetting(Setting setting)
        {
            if (HasSetting(setting.Key))
                throw new Exception("Setting already exist");

            Settings.Add(setting);
        }

        public bool HasSetting(string key)
        {
            // TODO: Add tests for casing
            return Settings.Any(KeyMatcher(key));
        }

        public void DeleteSetting(string settingKey)
        {
            if (!HasSetting(settingKey))
                return;

            Settings.RemoveAll(s => string.Equals(s.Key, settingKey, StringComparison.InvariantCultureIgnoreCase));
        }

        private static Func<Setting, bool> KeyMatcher(string key)
        {
            return s => string.Equals(s.Key, key, StringComparison.InvariantCultureIgnoreCase);
        }

        #region Equality comparison

        public bool Equals(Application other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Id, other.Id) && string.Equals(Name, other.Name) && Equals(Settings, other.Settings);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Application) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Id != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Settings != null ? Settings.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Application left, Application right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Application left, Application right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}