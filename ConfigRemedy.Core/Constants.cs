namespace ConfigRemedy.Core
{
    public static class Constants
    {
        public const string ApplicationName = "Configuratron";
        public const int DefaultPort = 2403;
        public const string DefaultLogsFolderName = "Logs";
        public const string DefaultDbFolderName = "Data";
        public const string DefaultHostname = "localhost";
        public const string ApiUrlBase = "/api";

        public const string AuthorizationTokenTemplate = "Token {0}";
        public const string AuthorizationHeaderName = "Authorization";
        public const string ApiKeyHeaderName = "X-Configuratron-ApiKey";
        public const string ApiKeyQueryStringName = "apiKey";

        public const string DefaultAdminPassword = "91076foo!Bar";
        public const string DefaultAdminDisplayName = "Administrator";
        public const string DefaultAdminUsername = "root";


        public const string ApiResourceUsers = "users";
        public const string ApiResourceEnvironments = "environments";
    }
}