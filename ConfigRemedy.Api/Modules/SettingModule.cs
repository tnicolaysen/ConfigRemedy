using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using ConfigRemedy.Domain;
using Nancy;
using Nancy.ModelBinding;
using Raven.Client;

namespace ConfigRemedy.Api.Modules
{
    public class SettingModule : BaseModule
    {
        public SettingModule(IDocumentStore docStore)
            : base("/environments/{envName}/{appName}")
        {
            Get["/settings"] = _ => // All app. settings in a given env.
            {
                string envName = RequiredParam(_, "envName");
                string appName = RequiredParam(_, "appName");

                using (var session = docStore.OpenSession())
                {
                    var env = GetEnvironment(session, envName);
                    var settings = SettingsAsDictionary(env, appName);

                    return Negotiate
                        .WithContentType("application/json")
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithModel(settings);
                }
            };

            Post["/settings"] = _ => // Create a new setting for a given app. in a given env.
            {
                string envName = RequiredParam(_, "envName");
                string appName = RequiredParam(_, "appName");

                using (var session = docStore.OpenSession())
                {
                    var env = GetEnvironment(session, envName);
                    var app = env.GetApplication(appName);
                    var setting = this.Bind<Setting>();

                    //if (app.HasSetting(setting.Key))
                    //{
                    //    return Negotiate.WithStatusCode(HttpStatusCode.Forbidden)
                    //                    .WithReasonPhrase("Duplicates are not allowed");
                    //}

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
                }
            };
        }

        private static Dictionary<string,string> SettingsAsDictionary(Environment env, string appName)
        {
            var settings = env.GetApplication(appName).Settings;
            return settings.ToDictionary(s => s.Key, s => s.Value);
        }


        private static Environment GetEnvironment(IDocumentSession session, string envName)
        {
            return session.Query<Environment>().Single(e => e.Name == envName);
        }

    }
}