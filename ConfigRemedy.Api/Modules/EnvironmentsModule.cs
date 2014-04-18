using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using Raven.Client;
using Raven.Client.Linq;
using Environment = ConfigRemedy.Domain.Environment;

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
                    return session.Query<Environment>().Select(e => e).ToList();
                }
            };

            Get["/{name}"] = _ => // Given environment
            {
                string name = RequiredParam(_, "name");

                using (var session = docStore.OpenSession())
                {
                    var environment = session.Query<Environment>().SingleOrDefault(env => env.Name == name);

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
                using (var session = docStore.OpenSession())
                {
                    var environment = this.Bind<Environment>(e => e.Id);
                    session.Store(environment);
                    session.SaveChanges();

                    // NOTE: Try to generalize this in time
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
    }
}