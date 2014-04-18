using System;
using System.Linq;
using System.Net.Mime;
using ConfigRemedy.Domain;
using Nancy;
using Nancy.Extensions;
using Nancy.ModelBinding;
using Nancy.Routing;
using Raven.Client;
using Raven.Client.Linq;
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
                return "[]";
            };

            Post["/"] = _ =>
            {
                string envName = _.envName;
                if (string.IsNullOrWhiteSpace(envName))
                    throw new ArgumentNullException("envName", "Required");

                using (var session = docStore.OpenSession())
                {
                    // get env
                    var env = session.Query<Environment>().Single(e => e.Name == envName);
                    var app = this.Bind<Application>(e => e.Id);

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
        }
    }
}