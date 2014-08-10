using System;
using System.Linq;
using ConfigRemedy.Core;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Security;

namespace ConfigRemedy.Security.Nancy
{
    /// <summary>
    /// Configuratron Token and API key based authentication implementation
    /// </summary>
    public static class ConfiguratronAuthentication
    {
        private const string Scheme = "Token";

        /// <summary>
        /// Enables Token authentication for the application
        /// </summary>
        /// <param name="pipelines">Pipelines to add handlers to (usually "this")</param>
        /// <param name="configuration">Forms authentication configuration</param>
        public static void Enable(IPipelines pipelines, ConfiguratronAuthenticationConfiguration configuration)
        {
            if (pipelines == null)
            {
                throw new ArgumentNullException("pipelines");
            }

            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            pipelines.BeforeRequest.AddItemToStartOfPipeline(GetCredentialRetrievalHook(configuration));
        }

        /// <summary>
        /// Enables Token authentication for a module
        /// </summary>
        /// <param name="module">Module to add handlers to (usually "this")</param>
        /// <param name="configuration">Forms authentication configuration</param>
        public static void Enable(INancyModule module, ConfiguratronAuthenticationConfiguration configuration)
        {
            if (module == null)
            {
                throw new ArgumentNullException("module");
            }

            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            module.RequiresAuthentication();
            module.Before.AddItemToStartOfPipeline(GetCredentialRetrievalHook(configuration));
        }

        /// <summary>
        /// Gets the pre request hook for loading the authenticated user's details
        /// from the auth header.
        /// </summary>
        /// <param name="configuration">Token authentication configuration to use</param>
        /// <returns>Pre request hook delegate</returns>
        private static Func<NancyContext, Response> GetCredentialRetrievalHook(ConfiguratronAuthenticationConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            return context =>
            {
                RetrieveCredentials(context, configuration);
                return null;
            };
        }

        private static void RetrieveCredentials(NancyContext context, ConfiguratronAuthenticationConfiguration configuration)
        {
            var token = ExtractTokenFromHeader(context.Request);
            if (token != null)
            {
                var user = configuration.Tokenizer.Detokenize(token, context);
                if (user != null)
                {
                    context.CurrentUser = user;
                }
            }
            else
            {
                var apiKey = ExtractApiKeyFromHeader(context.Request);
                if (apiKey == null)
                {
                    apiKey = ExtractApiKeyFromQueryString(context.Request);
                    if (apiKey == null)
                        return;
                }
                var user = configuration.UserResolver.GetUser(apiKey);
                if (user != null)
                {
                    context.CurrentUser = user;
                }
            }
        }

        private static string ExtractApiKeyFromHeader(Request request)
        {
            var apiKeyHeader = request.Headers[Constants.ApiKeyHeaderName];
            if (!apiKeyHeader.Any())
                return null;
            
            var apiKey = apiKeyHeader.First();

            return string.IsNullOrEmpty(apiKey) ? null : apiKey;
        }

        private static string ExtractApiKeyFromQueryString(Request request)
        {
            if (request.Query == null)
                return null;

            var apiKey = request.Query.apiKey;

            return string.IsNullOrEmpty(apiKey) ? null : apiKey;
        }

        private static string ExtractTokenFromHeader(Request request)
        {
            var authorization = request.Headers.Authorization;

            if (string.IsNullOrEmpty(authorization))
            {
                return null;
            }

            if (!authorization.StartsWith(Scheme))
            {
                return null;
            }

            try
            {
                var encodedToken = authorization.Substring(Scheme.Length).Trim();
                return String.IsNullOrWhiteSpace(encodedToken) ? null : encodedToken;
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}


