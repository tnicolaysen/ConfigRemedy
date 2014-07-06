using System;
using System.Collections.Generic;
using System.Linq;
using ConfigRemedy.Domain;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using Raven.Client;
using Environment = ConfigRemedy.Domain.Environment;

namespace ConfigRemedy.Api.Modules
{
    public class SettingModule : BaseModule
    {
        public SettingModule(IDocumentSession session)
            : base("/environments/{envName}/{appName}")
        {
            Get["/settings"] = _ => // All app. settings in a given env.
            {
                string envName = RequiredParam(_, "envName");
                string appName = RequiredParam(_, "appName");
                
                var env = GetEnvironment(session, envName);
                var settings = SettingsAsDictionary(env, appName);

                return Negotiate
                    .WithContentType("application/json")
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithModel(settings);
            };

            Get["/{settingKey}"] = _ => // Get the value of a specific key
            {
                string envName = RequiredParam(_, "envName");
                string appName = RequiredParam(_, "appName");
                string settingKey = RequiredParam(_, "settingKey");
                
                var env = GetEnvironment(session, envName);
                var app = env.GetApplication(appName);

                if (!app.HasSetting(settingKey))
                    return new NotFoundResponse();

                return new TextResponse(app.GetSetting(settingKey).Value);
            };

            Post["/settings"] = _ => // Create a new setting for a given app. in a given env.
            {
                string envName = RequiredParam(_, "envName");
                string appName = RequiredParam(_, "appName");

                var env = GetEnvironment(session, envName);
                var app = env.GetApplication(appName);
                var setting = this.Bind<Setting>();

                if (app.HasSetting(setting.Key))
                {
                    return Negotiate.WithStatusCode(HttpStatusCode.Forbidden)
                        .WithReasonPhrase("Duplicates are not allowed");
                }

                app.AddSetting(setting);

                session.Store(env);
                session.SaveChanges();

                // NOTE: Try to generalize this in time
                var modulePath = ModulePath.Replace("{envName}", envName)
                    .Replace("{appName}", appName);

                return Negotiate
                    .WithContentType("application/json")
                    .WithModel(setting) // TODO: Test for object structure
                    .WithHeader("Location", string.Format("{0}/{1}", modulePath, setting.Key))
                    .WithStatusCode(HttpStatusCode.Created);
            };
        }

        private static Dictionary<string,string> SettingsAsDictionary(Environment env, string appName)
        {
            var settings = env.GetApplication(appName).Settings;
            return settings.ToDictionary(s => s.Key, s => s.Value);
        }

        private static Func<Setting, bool> KeyMatcher(string key)
        {
            return s => string.Equals(s.Key, key, StringComparison.InvariantCultureIgnoreCase);
        }

        private static Environment GetEnvironment(IDocumentSession session, string envName)
        {
            return session.Query<Environment>().Single(e => e.ShortName == envName);
        }

    }
}