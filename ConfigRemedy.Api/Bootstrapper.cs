using System.IO;
using ConfigRemedy.Api.Annotations;
using ConfigRemedy.Api.Infrastructure;
using ConfigRemedy.Api.Infrastructure.Settings;
using Nancy.Bootstrapper;
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
        }
        
        /// <summary>
        /// Belived to behave as "singletons"
        /// </summary>
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            container.Register(RavenFactory.Create());
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            var documentStore = container.Resolve<IDocumentStore>();
            container.Register(documentStore.OpenSession());
        }

        public static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole()
                .WriteTo.RollingFile(Path.Combine(Settings.LogPath, "$log-{Date}.txt"))
                .CreateLogger();
        }
    }
}