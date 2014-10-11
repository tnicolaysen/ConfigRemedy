using System.IO;
using ConfigRemedy.Api.Annotations;
using ConfigRemedy.Api.Infrastructure;
using ConfigRemedy.Core.Configuration.Settings;
using ConfigRemedy.Repository;
using ConfigRemedy.Repository.Infrastructure;
using ConfigRemedy.Security;
using ConfigRemedy.Security.Nancy;
using Nancy.Authentication.Token;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Cryptography;
using Nancy.Diagnostics;
using Nancy.TinyIoc;
using Raven.Client;
using Serilog;

namespace ConfigRemedy.Api
{
    using Nancy;

    [UsedImplicitly]
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            CustomPipelines.Configure(pipelines);
            if(Settings.RestoreDefaultAdminAtStartup)
            {
                var adminAccountRestorer = container.Resolve<AdminAccountRestorer>();
                adminAccountRestorer.RestoreDefaultAdminAccount();
            }
        }
        
        /// <summary>
        /// Belived to behave as "singletons"
        /// </summary>
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            RegisterApplicationLevelComponents(container);
            container.Register(RavenFactory.Create());
        }

        public static void RegisterApplicationLevelComponents(TinyIoCContainer container)
        {
            container.Register<AdminAccountRestorer>();
            container.Register<ApiKeyProvider>();
            container.Register<UserRegistrationService>();
            container.Register<IPasswordHasher, SecurePasswordHasher>();
            container.Register<IStringHasher, Md5StringHasher>();
            container.Register<IHmacProvider, DefaultHmacProvider>();
            container.Register<IKeyGenerator, RandomKeyGenerator>();
        }

        public static void RegisterRequestLevelComponents(TinyIoCContainer container)
        {
            container.Register<IUserRepository, UserRepository>();
            container.Register<IApiKeyRepository, ApiKeyRepository>();
            container.Register<IUserResolver, UserResolver>();
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            var documentStore = container.Resolve<IDocumentStore>();
            var documentSession = documentStore.OpenSession();
            container.Register(documentSession);

            // TODO: check maybe there is a better way to register components, that depend to IDocumentSession
            // If they registered inside ConfigureApplicationContainer method, and Module has a dependency to, 
            // for instance IUserRepository, then IoC can't resolve IDocumentSession. But it has no problem resolving 
            // IDocumentSession at module level...

            RegisterRequestLevelComponents(container);
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            var tokenizer = container.Resolve<ITokenizer>();
            var userResolver = container.Resolve<IUserResolver>();

            ConfiguratronAuthentication.Enable(pipelines,
                new ConfiguratronAuthenticationConfiguration(tokenizer, userResolver));
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("app", @"app"));
            base.ConfigureConventions(nancyConventions);
        }

        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            // You reach the dashboard by pointing your browser to http://<address-of-your-application>/_Nancy/. 
            // However before being able to use the dashboard, you first need to configure it.
            get { return new DiagnosticsConfiguration { Password = @"foobar" }; }
        }

        public static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole()
                .WriteTo.RollingFile(Path.Combine(Settings.LogPath, "log-{Date}.txt"))
                .CreateLogger();
        }
    }
}