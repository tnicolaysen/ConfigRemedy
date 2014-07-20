using System;
using System.IO;

namespace ConfigRemedy.Api.Infrastructure.Settings
{
    public static class Settings
    {
        static Settings()
        {
            DbPath = GetDbPath();
            LogPath = GetLogPath();
        }

        private static string GetDbPath()
        {
            var commonAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var defaultDbPath = Path.Combine(commonAppData, "Configuratron", "Data");

            return SettingsReader<string>.Read("DbPath", defaultDbPath);
        }

        private static string GetLogPath()
        {
            var commonAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var defaultFolderPath = Path.Combine(commonAppData, "Configuratron", "Logs");

            return Environment.ExpandEnvironmentVariables(
                SettingsReader<string>.Read("LogPath", defaultFolderPath)
            );
        }

        public static string ApiUrl
        {
            get { return string.Format("http://{0}:{1}/", Hostname, Port); }
        }

        public static readonly int Port = SettingsReader<int>.Read("Port", 2403);
        public static readonly string Hostname = SettingsReader<string>.Read("Hostname", "+");
        public static readonly string DbPath;
        public static readonly string LogPath;
    }
}