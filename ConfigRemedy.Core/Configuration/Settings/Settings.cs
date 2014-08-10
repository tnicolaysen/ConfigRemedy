using System;
using System.IO;

namespace ConfigRemedy.Core.Configuration.Settings
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
            var defaultDbPath = Path.Combine(commonAppData, Constants.ApplicationName, Constants.DefaultDbFolderName);

            return SettingsReader<string>.Read("DbPath", defaultDbPath);
        }

        private static string GetLogPath()
        {
            var commonAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var defaultFolderPath = Path.Combine(commonAppData, Constants.ApplicationName, Constants.DefaultLogsFolderName);

            return Environment.ExpandEnvironmentVariables(
                SettingsReader<string>.Read("LogPath", defaultFolderPath)
            );
        }

        public static string ApiUrl
        {
            get { return string.Format("http://{0}:{1}/", Hostname, Port); }
        }

        public static readonly bool RestoreDefaultAdminAtStartup = SettingsReader<bool>.Read("RestoreDefaultAdminAtStartup", true);
        public static readonly int Port = SettingsReader<int>.Read("Port", Constants.DefaultPort);
        public static readonly string Hostname = SettingsReader<string>.Read("Hostname", Constants.DefaultHostname);
        public static readonly string DbPath;
        public static readonly string LogPath;
    }
}