using System.Linq;
using ConfigRemedy.Domain;
using Nancy;
using Nancy.ModelBinding;
using Raven.Client;

namespace ConfigRemedy.Api.Modules
{
    public class EnviromentModule : BaseModule
    {
        public EnviromentModule(IDocumentStore docStore)
            : base("/environments")
        {
            Get["/"] = _ => // All environments
            {
                using (var session = docStore.OpenSession())
                {
                    return session.Query<Environment>().ToList();
                }
            };

            Get["/{name}"] = _ => // Given environment
            {
                string name = RequiredParam(_, "name");

                using (var session = docStore.OpenSession())
                {
                    var environment = GetEnvironment(session, name);

                    if (environment == null)
                        return HttpStatusCode.NotFound;

                    return Negotiate
                        .WithContentType("application/json")
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithModel(environment);
                }
            };

            Post["/"] = _ =>
            {
                var environment = this.Bind<Environment>(e => e.Id);

                using (var session = docStore.OpenSession())
                {
                    if (GetEnvironment(session, environment.Name) != null)
                    {
                        return Negotiate.WithStatusCode(HttpStatusCode.Forbidden)
                                        .WithReasonPhrase("Duplicates are not allowed");
                    }

                    session.Store(environment);
                    session.SaveChanges();

                    return Negotiate
                        .WithContentType("application/json")
                        .WithModel(environment)
                        .WithHeader("Location", string.Format("{0}/{1}", ModulePath, environment.Name))
                        .WithStatusCode(HttpStatusCode.Created);
                }
            };

            Delete["/{name}"] = _ =>
            {
                var name = (string) _.name;

                using (var session = docStore.OpenSession())
                {
                    var envToDelete = session.Query<Environment>().SingleOrDefault(env => env.Name == name);

                    if (envToDelete == null)
                        return HttpStatusCode.NotFound;

                    session.Delete(envToDelete);
                    session.SaveChanges();

                    return HttpStatusCode.NoContent;
                }
            };
        }

        private Environment GetEnvironment(IDocumentSession session, string name)
        {
            return session.Load<Environment>("environments/" + name);
        }
    }
}