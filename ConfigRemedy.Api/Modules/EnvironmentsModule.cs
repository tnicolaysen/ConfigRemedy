using ConfigRemedy.Api.Annotations;
using ConfigRemedy.Core;
using ConfigRemedy.Security.Modules;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using Raven.Client;
using System;
using System.Linq;
using Environment = ConfigRemedy.Domain.Environment;

namespace ConfigRemedy.Api.Modules
{
    [UsedImplicitly]
    public class EnviromentModule : AuthenticatedModule
    {
        private readonly IDocumentSession _session;

        public EnviromentModule(IDocumentSession session)
        {
            _session = session;
            Get[Constants.ApiResourceEnvironments] = _ => GetAllEnvironments();
            Get[Constants.ApiResourceEnvironments + "/{name}"] = _ => GetEnvironment(_.name);
            Post[Constants.ApiResourceEnvironments] = _ => CreateEnvironment();
            Put[Constants.ApiResourceEnvironments + "/{name}"] = _ => UpdateEnvironment(_.name); // TODO: Add tests
            Delete[Constants.ApiResourceEnvironments + "/{name}"] = _ => DeleteEnvironment(_.name); 
        }

        private dynamic DeleteEnvironment(string name)
        {
            Guard.NotNullOrEmpty(() => name, name);

            var envToDelete = _session.Query<Environment>().SingleOrDefault(env => env.ShortName == name);

            if (envToDelete == null)
                return HttpStatusCode.NotFound;

            _session.Delete(envToDelete);
            _session.SaveChanges();

            return HttpStatusCode.NoContent;
        }

        private dynamic UpdateEnvironment(string name)
        {
            Guard.NotNullOrEmpty(() => name, name);

            var updatedEnvironment = this.Bind<Environment>(e => e.Id);

            var storedEnvironment = GetEnvironment(_session, name);
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

            _session.Store(storedEnvironment);
            _session.SaveChanges();

            return storedEnvironment;
        }

        private dynamic CreateEnvironment()
        {
            var environment = this.Bind<Environment>(e => e.Id);

            if (GetEnvironment(_session, environment.ShortName) != null)
            {
                return new TextResponse(HttpStatusCode.Forbidden, "Duplicates are not allowed")
                {
                    ReasonPhrase = "Duplicates are not allowed"
                };
            }

            _session.Store(environment);
            _session.SaveChanges();

            return Negotiate
                .WithModel(environment)
                .WithHeader("Location", Request.Path + environment.ShortName)
                .WithStatusCode(HttpStatusCode.Created);
        }

        private dynamic GetEnvironment(string name)
        {
            Guard.NotNullOrEmpty(() => name, name);

            var environment = GetEnvironment(_session, name);

            if (environment == null)
                return HttpStatusCode.NotFound;

            return environment;
        }

        private dynamic GetAllEnvironments()
        {
            var environments = _session.Query<Environment>()
                .Customize(c => c.WaitForNonStaleResultsAsOfNow(TimeSpan.FromSeconds(5)))
                .ToList();

            environments.Sort();

            var envProjection = environments
                .Select(e => new
                {
                    e.ShortName,
                    e.LongName,
                    e.Description,
                    e.Icon,
                    Link = CreateLinkForEnvironment(e)
                });

            return envProjection;
        }

        private string CreateLinkForEnvironment(Environment environment)
        {
            return string.Format("/{0}/{1}", Constants.ApiResourceEnvironments, environment.ShortName);
        }

        private Environment GetEnvironment(IDocumentSession session, string name)
        {
            return session.Load<Environment>("environments/" + name);
        }
    }
}