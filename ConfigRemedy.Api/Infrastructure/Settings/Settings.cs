using System;
using System.IO;

namespace ConfigRemedy.Api.Infrastructure.Settings
{
    public static class Settings
    {
        static Settings()
        {
            DbPath = GetDbPath();
        }

        private static string GetDbPath()
        {
            var defaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "Configuratron", "Data");

            return SettingsReader<string>.Read("DbPath", defaultPath);
        }

        public static string ApiUrl
        {
            get
            {
                return string.Format("http://{0}:{1}/", Hostname, Port);
            }
        }

        public static readonly string LogPath =
            Environment.ExpandEnvironmentVariables(SettingsReader<string>.Read("LogPath",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    "Configuratron", "Logs")));

        public static int Port = SettingsReader<int>.Read("Port", 2403);
        public static string Hostname = SettingsReader<string>.Read("Hostname", "+");
        public static string DbPath;
  
    }
}