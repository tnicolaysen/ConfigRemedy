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
                    .Customize(c => c.WaitForNonStaleResultsAsOfNow(TimeSpan.FromSeconds(5)))
                    .ToList();
                
                environments.Sort();

                var envProjection = environments
                    //.OrderBy(e => e.ShortName)
                    .Select(e => new
                    {
                        e.ShortName,
                        e.LongName,
                        e.Description,
                        e.Icon,
                        Link = CreateLinkForEnvironment(e)
                    });

                return envProjection;
            };

            Get["/{name}"] = _ => // Given environment
            {
                string name = RequiredParam(_, "name");

                var environment = GetEnvironment(session, name);

                if (environment == null)
                    return HttpStatusCode.NotFound;

                return environment;
            };

            Post["/"] = _ =>
            {
                var environment = this.Bind<Environment>(e => e.Id);

                if (GetEnvironment(session, environment.ShortName) != null)
                {
                    return new TextResponse(HttpStatusCode.Forbidden, "Duplicates are not allowed")
                    {
                        ReasonPhrase = "Duplicates are not allowed"
                    };
                }

                session.Store(environment);
                session.SaveChanges();
                    
                return Negotiate
                    .WithModel(environment)
                    .WithHeader("Location", string.Format("{0}/{1}", ModulePath, environment.ShortName))
                    .WithStatusCode(HttpStatusCode.Created);
            };

            // TODO: Add tests
            Put["/{name}"] = _ =>
            {
                string name = RequiredParam(_, "name");
                var updatedEnvironment = this.Bind<Environment>(e => e.Id);

                var storedEnvironment = GetEnvironment(session, name);
                if (storedEnvironment == null)
                {
                    return new TextResponse(HttpStatusCode.NotFound, "Environment not found. Cannot update.")
                    {
                        ReasonPhrase = "Environment not found. Cannot update."
                    };
                }

                if (string.Equals(storedEnvironment.ShortName, updatedEnvironment.ShortName,
                                  StringComparison.InvariantCultureIgnoreCase))
                {
                    return new TextResponse(HttpStatusCode.BadRequest, "Attempting to update 'shortName'. This is illegal as it's the ID.")
                    {
                        ReasonPhrase = "Attempting to update 'shortName'. This is illegal as it's the ID."
                    };
                }

                storedEnvironment.Icon = updatedEnvironment.Icon;
                storedEnvironment.LongName = updatedEnvironment.LongName;
                storedEnvironment.Description = updatedEnvironment.Description;

                session.Store(storedEnvironment);
                session.SaveChanges();

                return storedEnvironment;
            };

            Delete["/{name}"] = _ =>
            {
                var name = (string) _.name;
                
                var envToDelete = session.Query<Environment>().SingleOrDefault(env => env.ShortName == name);

                if (envToDelete == null)
                    return HttpStatusCode.NotFound;

                session.Delete(envToDelete);
                session.SaveChanges();

                return HttpStatusCode.NoContent;
            };
        }

        private string CreateLinkForEnvironment(Environment environment)
        {
            return string.Format("/environments/{0}", environment.ShortName);
        }

        private Environment GetEnvironment(IDocumentSession session, string name)
        {
            return session.Load<Environment>("environments/" + name);
        }
    }
}