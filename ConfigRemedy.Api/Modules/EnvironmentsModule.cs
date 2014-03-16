using System.Linq;
using ConfigRemedy.Api.Modules;
using ConfigRemedy.Models;

namespace ConfigRemedy.Api
{
    public class EnviromentModule : RavenDbModule
    {
        public EnviromentModule() : base("/environments")
        {
            Get["/"] = _ =>
            {
                using (DbSession = Store.OpenSession())
                {
                    return DbSession.Query<Environment>().Select(e => e).ToList();
                }
            };

            Post["/{name}"] = _ =>
            {
                using (DbSession = Store.OpenSession())
                {
                    var environment = new Environment {EnvironmentName = _.name};
                    DbSession.Store(environment);
                    DbSession.SaveChanges();
                    return environment;
                }
            };
        }
    }
}