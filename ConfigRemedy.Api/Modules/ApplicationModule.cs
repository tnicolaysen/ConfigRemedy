using System;
using System.Collections.Generic;
using System.Linq;
using ConfigRemedy.Domain;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using Raven.Client;
using Raven.Client.Linq;
using Environment = ConfigRemedy.Domain.Environment;

namespace ConfigRemedy.Api.Modules
{
    public class ApplicationModule : BaseModule
    {
        public ApplicationModule(IDocumentSession session)
            : base("/applications")
        {
            Get["/"] = _ => // All apps 
            {
                var applications = session.Query<Application>()
                    .Customize(c => c.WaitForNonStaleResultsAsOfNow(TimeSpan.FromSeconds(5)))
                    .ToList();
                
                var environments = session.Query<Environment>().ToList();

                var appProjection = applications.Select(a => new
                {
                    a.Id,
                    a.Name,
                    Settings = PadSettings(a.Settings, environments),
                    Link = CreateLinkForApplication(a)
                });

                return appProjection;
            };

            Get["/{appName}"] = _ => // Specific app
            {
                string appName = RequiredParam(_, "appName");

                var app = GetApplication(session, appName);

                if (app == null)
                    return Negotiate.WithStatusCode(HttpStatusCode.NotFound);

                var environments = session.Query<Environment>().ToList();
                app.Settings = PadSettings(app.Settings, environments);

                return app;
            };

            Post["/"] = _ => // Create a new app
            {
                var app = this.Bind<Application>(a => a.Id);

                if (GetApplication(session, app.Name) != null)
                {
                    return new TextResponse(HttpStatusCode.Forbidden, "Duplicates are not allowed")
                    {
                        ReasonPhrase = "Duplicates are not allowed"
                    };
                }

                session.Store(app);
                session.SaveChanges();

                return Negotiate
                    .WithModel(app)
                    .WithHeader("Location", string.Format("{0}/{1}", ModulePath, app.Name))
                    .WithStatusCode(HttpStatusCode.Created);
            };

            Delete["/{appName}"] = _ => // Delete a given app.
            {
                string appName = RequiredParam(_, "appName");

                var appToDelete = GetApplication(session, appName);

                if (appToDelete == null)
                {
                    return HttpStatusCode.NotFound;
                }

                session.Delete(appToDelete);
                session.SaveChanges();

                return HttpStatusCode.NoContent;
            };
        }

        private List<Setting> PadSettings(List<Setting> settings, IEnumerable<Environment> environments)
        {
            foreach (var environment in environments)
            {
                foreach (var setting in settings)
                {
                    var envName = environment.ShortName.ToLowerInvariant();
                    if (!setting.Overrides.ContainsKey(envName))
                        setting.Overrides[envName] = null;
                }
            }
            
            return settings;
        }


        private string CreateLinkForApplication(Application application)
        {
            return string.Format("/application/{0}", application.Name);
        }

        private static Application GetApplication(IDocumentSession session, string appName)
        {
            return session.Load<Application>("applications/" + appName);
        }
    }
}