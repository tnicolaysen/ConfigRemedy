using System;
using System.Diagnostics;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using Raven.Client;
using Raven.Client.Linq;
using Environment = ConfigRemedy.Models.Environment;

namespace ConfigRemedy.Api.Modules
{
    public class EnviromentModule : NancyModule
    {
        public EnviromentModule() : this(RavenFactory.Create())
        {
        }

        public EnviromentModule(IDocumentStore docStore)
            : base("/environments")
        {
            Get["/"] = _ =>
            {
                using (var session  = docStore.OpenSession())
                {
                    return session.Query<Environment>().Select(e => e).ToList();
                }
            };

            Post["/"] = _ =>
            {
                using (var session = docStore.OpenSession())
                {
                    var environment = this.Bind<Environment>(e => e.Id);
                    session.Store(environment);
                    session.SaveChanges();

                    return HttpStatusCode.Created;
                }
            };

            Delete["/{name}"] = _ =>
            {
                var name = (string)_.name;

                using (var session = docStore.OpenSession())
                {
                    var envToDelete = session.Query<Environment>()
                                             .SingleOrDefault(env => env.Name == name);

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