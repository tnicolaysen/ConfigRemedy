using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConfigRemedy.Api.Modules;
using ConfigRemedy.Models;
using Raven.Client;

namespace ConfigRemedy.Api
{
    using Nancy;

    public class EnviromentModule : RavenDbModule
    {
        public EnviromentModule() : base("/environments")
        {
            Get["/"] = _ =>
            {
                using (Session = Store.OpenSession())
                {
                    return Session.Query<Environment>().Select(e => e).ToList();
                }
            };

            Post["/{name}"] = _ =>
            {
                using (Session = Store.OpenSession())
                {
                    var environment = new Environment {EnvironmentName = _.name};
                    Session.Store(environment);
                    Session.SaveChanges();
                    return environment;
                }
            };
        }
    }
}