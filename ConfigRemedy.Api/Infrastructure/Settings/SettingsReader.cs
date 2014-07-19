using System;
using System.Configuration;

namespace ConfigRemedy.Api.Infrastructure.Settings
{
    public class SettingsReader<T>
    {
        public static T Read(string name, T defaultValue = default(T))
        {
            var fullKey =  "Configuratron/" + name;

            if (ConfigurationManager.AppSettings[fullKey] != null)
            {
                return (T)Convert.ChangeType(ConfigurationManager.AppSettings[fullKey], typeof(T));
            }

            return defaultValue;
        }
    }
}