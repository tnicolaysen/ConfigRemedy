using System.Linq;
using ConfigRemedy.Domain;
using Nancy;
using Raven.Client;

namespace ConfigRemedy.Api.Modules
{
    public class SettingModule : BaseModule
    {
        public SettingModule(IDocumentStore docStore)
            : base("/environments/{envName}/applications/{appName}/settings")
        {
            Get["/"] = _ => // All app. settings in a given env.
            {
                string envName = RequiredParam(_, "envName");
                string appName = RequiredParam(_, "appName");

                using (var session = docStore.OpenSession())
                {
                    var env = GetEnvironment(session, envName);

                    return Negotiate
                        .WithContentType("application/json")
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithModel(env.GetApplication(appName).Settings);
                }
            };
        }

        private static Environment GetEnvironment(IDocumentSession session, string envName)
        {
            return session.Query<Environment>().Single(e => e.Name == envName);
        }

    }
}