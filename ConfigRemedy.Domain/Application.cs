using System;
using System.Collections.Generic;
using System.Linq;

namespace ConfigRemedy.Domain
{
    public class Application
    {
        public string Name { get; set; }
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
                throw new Exception("Duplicate applications found");

            Settings.Add(setting);
        }

        public void AddSetting(string key, string value)
        {
            AddSetting(new Setting {Key = key, Value = value});
        }

        public bool HasSetting(string key)
        {
            // TODO: Add tests for casing
            return Settings.Any(KeyMatcher(key));
        }

        private static Func<Setting, bool> KeyMatcher(string key)
        {
            return s => string.Equals(s.Key, key, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}