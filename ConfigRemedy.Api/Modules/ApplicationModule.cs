using ConfigRemedy.Api.Annotations;
using ConfigRemedy.Core;
using ConfigRemedy.Core.Modules;
using ConfigRemedy.Domain;
using ConfigRemedy.Security.Modules;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using Environment = ConfigRemedy.Domain.Environment;

namespace ConfigRemedy.Api.Modules
{
    [UsedImplicitly]
    public class ApplicationModule : BaseModule
    {
        private readonly IDocumentSession _session;

        public ApplicationModule(IDocumentSession session)
        {
            _session = session;

            Get["applications/"] = _ => GetAllApplications();
            Get["applications/{appName}"] = _ => GetApplication(_.appName);
            Post["applications/"] = _ => CreateApplication();
            Delete["applications/{appName}"] = _ => DeleteApplication(_.appName);
        }

        private dynamic DeleteApplication(string appName)
        {
            Guard.NotNullOrEmpty(() => appName, appName);

            var appToDelete = GetApplication(_session, appName);

            if (appToDelete == null)
            {
                return HttpStatusCode.NotFound;
            }

            _session.Delete(appToDelete);
            _session.SaveChanges();

            return HttpStatusCode.NoContent;
        }

        private dynamic CreateApplication()
        {
            var app = this.Bind<Application>(a => a.Id);

            if (GetApplication(_session, app.Name) != null)
            {
                return new TextResponse(HttpStatusCode.Forbidden, "Duplicates are not allowed")
                {
                    ReasonPhrase = "Duplicates are not allowed"
                };
            }

            _session.Store(app);
            _session.SaveChanges();

            return Negotiate
                .WithModel(app)
                .WithHeader("Location", Request.Path + app.Name)
                .WithStatusCode(HttpStatusCode.Created);
        }

        private dynamic GetApplication(string appName)
        {
            Guard.NotNullOrEmpty(() => appName, appName);

            var app = GetApplication(_session, appName);

            if (app == null)
                return Negotiate.WithStatusCode(HttpStatusCode.NotFound);

            var environments = _session.Query<Environment>().ToList();
            app.Settings = PadSettings(app.Settings, environments);

            return app;
        }

        private dynamic GetAllApplications()
        {
            var applications = _session.Query<Application>()
                .Customize(c => c.WaitForNonStaleResultsAsOfNow(TimeSpan.FromSeconds(5)))
                .ToList();

            var environments = _session.Query<Environment>().ToList();

            var appProjection = applications.Select(a => new
            {
                a.Id,
                a.Name,
                Settings = PadSettings(a.Settings, environments),
                Link = CreateLinkForApplication(a)
            });

            return appProjection;
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