using System.Linq;
using ConfigRemedy.Domain;
using Nancy;
using Nancy.ModelBinding;
using Raven.Client;
using Environment = ConfigRemedy.Domain.Environment;

namespace ConfigRemedy.Api.Modules
{
    public class ApplicationModule : BaseModule
    {
        public ApplicationModule(IDocumentStore docStore)
            : base("/environments/{envName}/applications")
        {
            Get["/"] = _ => // All apps in a given env.
            {
                string envName = RequiredParam(_, "envName");

                using (var session = docStore.OpenSession())
                {
                    var env = GetEnvironment(session, envName);

                    return Negotiate
                        .WithContentType("application/json")
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithModel(env.Applications);
                }
            };

            Get["/{appName}"] = _ => // Specific app
            {
                string envName = RequiredParam(_, "envName");
                string appName = RequiredParam(_, "appName");

                using (var session = docStore.OpenSession())
                {
                    var env = GetEnvironment(session, envName);

                    if (!env.HasApplication(appName))
                        return Negotiate.WithStatusCode(HttpStatusCode.NotFound);

                    return Negotiate
                        .WithContentType("application/json")
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithModel(env.GetApplication(appName));
                }
            };

            Post["/"] = _ => // Create a new app. in a given env.
            {
                string envName = RequiredParam(_, "envName");

                using (var session = docStore.OpenSession())
                {
                    var env = GetEnvironment(session, envName);
                    var app = this.Bind<Application>();

                    if (env.HasApplication(app.Name))
                    {
                        return Negotiate.WithStatusCode(HttpStatusCode.Forbidden)
                                        .WithReasonPhrase("Duplicates are not allowed");
                    }

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

            Delete["/{appName}"] = _ => // Delete a given app. in a given env.
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

        private static Environment GetEnvironment(IDocumentSession session, string envName)
        {
            return session.Load<Environment>("environments/" + envName);
        }
    }
}