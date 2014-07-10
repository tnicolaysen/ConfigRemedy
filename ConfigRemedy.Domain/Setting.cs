using System;
using System.Collections.Generic;

namespace ConfigRemedy.Domain
{
    public class Setting : IEquatable<Setting>
    {
        public string Key { get; set; }
        public string DefaultValue { get; set; }
        public string Description { get; set; }
        public bool Deleted { get; set; }
        public SortedDictionary<string, string> Overrides { get; private set; }
        public IList<SettingHistory> History { get; private set; }

        public Setting()
        {
            Overrides = new SortedDictionary<string, string>();
            History = new List<SettingHistory>();
        }

        public Setting(string key, string defaultValue) : this()
        {
            Key = key;
            DefaultValue = defaultValue;
        }

        public void SetValueInEnvironment(string envName, SettingOverride settingOverride)
        {
            Overrides[envName.ToLowerInvariant()] = settingOverride.Value;
        }

        public string GetValueForEnvironment(string envName)
        {
            if (Overrides.ContainsKey(envName))
            {
                return Overrides[envName];
            }

            return DefaultValue;
        }

        public void RemoveOverride(string envName)
        {
            Overrides.Remove(envName.ToLowerInvariant());

        }

        public bool Equals(Setting other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Key, other.Key) && string.Equals(DefaultValue, other.DefaultValue);
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
                return ((Key != null ? Key.GetHashCode() : 0)*397) ^
                       (DefaultValue != null ? DefaultValue.GetHashCode() : 0);
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
    }
}