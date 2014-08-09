
using ConfigRemedy.Domain;
using ConfigRemedy.Security.Domain;
using Raven.Abstractions.Util;
using Raven.Client;

namespace ConfigRemedy.Api.Infrastructure
{
    public static class RavenConfigurator
    {
        public static void Configure(IDocumentStore store)
        {
            RegisterCustomConventions(store);
        }

        private static void RegisterCustomConventions(IDocumentStore store)
        {
            // Create custom convetion for Environment so that they get a natural 
            // "unique" constraint. It makes it easier to look them up as well.
            store.Conventions.RegisterIdConvention<Environment>((dbName, commands, env) =>
                "environments/" + env.ShortName);
            
            store.Conventions.RegisterAsyncIdConvention<Environment>((dbName, commands, env) =>
                new CompletedTask<string>("environments/" + env.ShortName));

            // Create custom convetion for Application so that they get a natural 
            // "unique" constraint. It makes it easier to look them up as well.
            store.Conventions.RegisterIdConvention<Application>((dbName, commands, env) =>
                "applications/" + env.Name);

            store.Conventions.RegisterAsyncIdConvention<Application>((dbName, commands, env) =>
                new CompletedTask<string>("applications/" + env.Name));


            // Create custom convetion for User so that they get a natural 
            // "unique" constraint. It makes it easier to look them up as well.
            store.Conventions.RegisterIdConvention<User>((dbName, commands, user) =>
                "users/" + user.Username);

            store.Conventions.RegisterAsyncIdConvention<User>((dbName, commands, user) =>
                new CompletedTask<string>("users/" + user.Username));
        }
    }
}