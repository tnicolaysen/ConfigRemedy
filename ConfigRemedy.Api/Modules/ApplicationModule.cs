using System;
using System.Linq;
using ConfigRemedy.Domain;
using Nancy;
using Nancy.ModelBinding;
using Raven.Client;
using Environment = ConfigRemedy.Domain.Environment;

namespace ConfigRemedy.Api.Modules
{
    public class ApplicationModule :NancyModule
    {
        public ApplicationModule(IDocumentStore docStore)
            : base("/environments/{envName}/applications")
        {
            Get["/"] = _ =>
            {
                string envName = RequiredParam(_, "envName");

                using (var session = docStore.OpenSession())
                {
                    var env = GetEnvironment(session, envName);

                    return Negotiate
                        .WithContentType("application/json")
                        .WithModel(env.Applications);
                }
            };

            Post["/"] = _ =>
            {
                string envName = RequiredParam(_, "envName");

                using (var session = docStore.OpenSession())
                {
                    var env = GetEnvironment(session, envName);
                    var app = this.Bind<Application>();

                    // TODO: Check for dupes
                    env.Applications.Add(app);

                    session.Store(env);
                    session.SaveChanges();

                    // NOTE: Try to generalize this in time
                    var modulePath = ModulePath.Replace("{envName}", envName);
                    return Negotiate
                        .WithContentType("application/json")
                        .WithModel(app)
                        .WithHeader("Location", string.Format("{0}/{1}",  modulePath, app.Name))
                        .WithStatusCode(HttpStatusCode.Created);
                }
            };

            Delete["/{appName}"] = _ =>
            {
                string envName = RequiredParam(_, "envName");
                string appName = RequiredParam(_, "appName");

                using (var session = docStore.OpenSession())
                {
                    var envToModify = session.Query<Environment>()
                                             .SingleOrDefault(env => env.Name == envName);                                             

                    if (envToModify == null)
                        return HttpStatusCode.NotFound;

                    if (!envToModify.Applications.Any(a => a.Name == appName))
                        return HttpStatusCode.NotFound;

                    envToModify.Applications.RemoveAll(a => a.Name == appName);

                    session.Store(envToModify);
                    session.SaveChanges();

                    return HttpStatusCode.NoContent;
                }
            };
        }

        private string RequiredParam(dynamic _, string paramName)
        {
            string parmaAsString = _[paramName];
                
            if (string.IsNullOrWhiteSpace(parmaAsString))
                throw new ArgumentNullException("paramName", "Required param was null or empty string");

            return parmaAsString;
        }

        private static Environment GetEnvironment(IDocumentSession session, string envName)
        {
            return session.Query<Environment>()
                          .Single(e => e.Name == envName);
        }
    }
}