using System;
using System.Collections.Generic;
using ConfigRemedy.Domain.Annotations;

namespace ConfigRemedy.Domain
{
    [UsedImplicitly, MeansImplicitUse]
    public class Setting : IEquatable<Setting>
    {
        [NotNull] public string Key { get; set; }
        [NotNull] public string DefaultValue { get; set; }

        public string Description { get; set; }
        public bool Deleted { get; internal set; }
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

        #region Equality comparison

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
            if (obj.GetType() != GetType()) return false;
            return Equals((Setting) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Key.GetHashCode()*397) ^ DefaultValue.GetHashCode();
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