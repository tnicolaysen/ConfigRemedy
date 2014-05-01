using System;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using Raven.Client;
using Environment = ConfigRemedy.Domain.Environment;

namespace ConfigRemedy.Api.Modules
{
    public class EnviromentModule : BaseModule
    {
        public EnviromentModule(IDocumentSession session)
            : base("/environments")
        {
            Get["/"] = _ => // All environments
            {
                var environments = session.Query<Environment>()
                    .Customize(c => c.WaitForNonStaleResultsAsOfNow())
                    .ToList();

                var envProjection = environments.Select(e => new {Name = e.Name, Link = CreateLinkForEnvironment(e)});
                return Negotiate
                    .WithHeader("Access-Control-Allow-Origin", "*")
                    .WithModel(envProjection);
            };

            Get["/{name}"] = _ => // Given environment
            {
                string name = RequiredParam(_, "name");

                var environment = GetEnvironment(session, name);

                if (environment == null)
                    return HttpStatusCode.NotFound;

                return Negotiate
                    .WithHeader("Access-Control-Allow-Origin", "*")
                    .WithContentType("application/json")
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithModel(environment);
            };

            Post["/"] = _ =>
            {
                var environment = this.Bind<Environment>(e => e.Id);

                if (GetEnvironment(session, environment.Name) != null)
                {
                    // TODO: Refactor or try to find a better builder
                    var response = new TextResponse(HttpStatusCode.Forbidden, "Duplicates are not allowed");
                    response.WithHeader("Access-Control-Allow-Origin", "*");
                    response.ReasonPhrase = "Duplicates are not allowed";
                    return response;
                }

                session.Store(environment);
                session.SaveChanges();
                    
                return Negotiate
                    .WithHeader("Access-Control-Allow-Origin", "*")
                    .WithContentType("application/json")
                    .WithModel(environment)
                    .WithHeader("Location", string.Format("{0}/{1}", ModulePath, environment.Name))
                    .WithStatusCode(HttpStatusCode.Created);
            };

            Delete["/{name}"] = _ =>
            {
                var name = (string) _.name;
                
                var envToDelete = session.Query<Environment>().SingleOrDefault(env => env.Name == name);

                if (envToDelete == null)
                    return HttpStatusCode.NotFound;

                session.Delete(envToDelete);
                session.SaveChanges();

                return HttpStatusCode.NoContent;
            };
        }

        private string CreateLinkForEnvironment(Environment environment)
        {
            return string.Format("/environments/{0}", environment.Name);
        }

        private Environment GetEnvironment(IDocumentSession session, string name)
        {
            return session.Load<Environment>("environments/" + name);
        }
    }
}