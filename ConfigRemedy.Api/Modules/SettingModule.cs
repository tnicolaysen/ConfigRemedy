using System;
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
            : base("/applications/{appName}")
        {
            //Get["/settings"] = _ => // All app. settings 
            //{
            //    string appName = RequiredParam(_, "appName");

            //    var app = GetApplication(session, appName);
            //    var environments = session.Query<Environment>();

            //    foreach (var environment in environments)
            //    {
            //        foreach (var setting in app.Settings)
            //        {
            //            var envName = environment.ShortName.ToLowerInvariant();
            //            if (!setting.Overrides.ContainsKey(envName))
            //                setting.Overrides[envName] = null;
            //        }
            //    }

            //    return app.Settings;
            //};

            Get["/settings/{settingKey}"] = _ => // Get a given setting object
            {
                string appName = RequiredParam(_, "appName");
                string settingKey = RequiredParam(_, "settingKey");

                var app = GetApplication(session, appName);

                if (!app.HasSetting(settingKey))
                    return new NotFoundResponse();

                return app.GetSetting(settingKey);
            };

            Delete["/settings/{settingKey}"] = _ => // Delete a setting and all overrides
            {
                string appName = RequiredParam(_, "appName");
                string settingKey = RequiredParam(_, "settingKey");

                var app = GetApplication(session, appName);
                app.DeleteSetting(settingKey);

                session.Store(app);
                session.SaveChanges();

                return HttpStatusCode.NoContent;
            };

            Post["/settings"] = _ => // Create a new setting for a given app. 
            {
                string appName = RequiredParam(_, "appName");

                var app = GetApplication(session, appName);
                var setting = this.Bind<Setting>(s => s.Deleted, s => s.Overrides, s => s.History);

                if (app.HasSetting(setting.Key))
                {
                    return new TextResponse(HttpStatusCode.Forbidden, "Duplicates are not allowed")
                    {
                        ReasonPhrase = "Duplicates are not allowed"
                    };
                }

                app.AddSetting(setting);

                session.Store(app);
                session.SaveChanges();

                // NOTE: Try to generalize this in time
                var modulePath = ModulePath.Replace("{appName}", appName);

                return Negotiate
                    .WithModel(setting) // TODO: Test for object structure
                    .WithHeader("Location", string.Format("{0}/settings/{1}", modulePath, setting.Key))
                    .WithStatusCode(HttpStatusCode.Created);
            };

            Post["/settings/{envName}/"] = _ => // Create a setting value override for a given app. in a given env.
            {
                string envName = RequiredParam(_, "envName");
                string appName = RequiredParam(_, "appName");

                var settingOverride = this.Bind<SettingOverride>();

                var app = GetApplication(session, appName);
                if (app == null)
                {
                    return new TextResponse(HttpStatusCode.NotFound, "Application not found")
                    {
                        ReasonPhrase = "Application not found"
                    };
                }

                var env = GetEnvironment(session, envName);
                if (env == null)
                {
                    return new TextResponse(HttpStatusCode.NotFound, "Environment not found")
                    {
                        ReasonPhrase = "Environment not found"
                    };
                }

                var setting = app.GetSetting(settingOverride.Key);
                if (setting == null)
                {
                    return new TextResponse(HttpStatusCode.NotFound, "Setting not found")
                    {
                        ReasonPhrase = "Setting not found"
                    };
                }

                setting.SetValueInEnvironment(envName, settingOverride);

                session.Store(app);
                session.SaveChanges();

                // NOTE: Try to generalize this in time
                var modulePath = ModulePath.Replace("{appName}", appName);

                return Negotiate
                    .WithModel(setting) // TODO: Test for object structure
                    .WithHeader("Location", string.Format("{0}/settings/{1}/{2}", modulePath, envName, setting.Key))
                    .WithStatusCode(HttpStatusCode.Created);
            };

            Get["/settings/{envName}/{settingKey}"] = _ => // Get a setting value for a given app. in a given env.
            {
                string envName = RequiredParam(_, "envName");
                string appName = RequiredParam(_, "appName");
                string settingKey = RequiredParam(_, "settingKey");

                var app = GetApplication(session, appName);
                //var env = GetEnvironment(session, envName);

                var setting = app.GetSetting(settingKey);

                if (setting == null)
                {
                    return new NotFoundResponse();
                }

                return new TextResponse(setting.GetValueForEnvironment(envName));
            };

            Delete["/settings/{envName}/{settingKey}"] = _ => // Delete an override for a given app. in a given env.
            {
                string envName = RequiredParam(_, "envName");
                string appName = RequiredParam(_, "appName");
                string settingKey = RequiredParam(_, "settingKey");

                var app = GetApplication(session, appName);

                var setting = app.GetSetting(settingKey);

                if (setting == null)
                {
                    return new NotFoundResponse();
                }

                setting.RemoveOverride(envName);

                session.Store(setting);
                session.SaveChanges();

                return HttpStatusCode.NoContent;
            };
        }

        //private static Dictionary<string, string> SettingsAsDictionary(Environment env, string appName)
        //{
        //    var settings = env.GetApplication(appName).Settings;
        //    return settings.ToDictionary(s => s.Key, s => s.DefaultValue);
        //}

        private static Func<Setting, bool> KeyMatcher(string key)
        {
            return s => string.Equals(s.Key, key, StringComparison.InvariantCultureIgnoreCase);
        }

        private static Application GetApplication(IDocumentSession session, string appName)
        {
            return session.Load<Application>("applications/" + appName);
        }

        private static Environment GetEnvironment(IDocumentSession session, string envName)
        {
            return session.Load<Environment>("environments/" + envName);
        }
    }
}