using System.Linq;
using ConfigRemedy.Models;
using Nancy;
using Raven.Client;
using Raven.Client.Linq;

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

            Post["/{name}"] = _ =>
            {
                using (var session = docStore.OpenSession())
                {
                    var environment = new Environment {EnvironmentName = _.name};
                    session.Store(environment);
                    session.SaveChanges();
                    return environment;
                }
            };
        }
    }
}