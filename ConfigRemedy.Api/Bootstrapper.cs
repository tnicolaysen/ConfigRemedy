using ConfigRemedy.Api.Infrastructure;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Raven.Abstractions.Extensions;
using Raven.Client;

namespace ConfigRemedy.Api
{
    using Nancy;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
        }

        protected override void RequestStartup(TinyIoCContainer requestContainer, IPipelines pipelines, NancyContext context)
        {
            AddCrossOriginSupport(pipelines);
        }

        private static void AddCrossOriginSupport(IPipelines pipelines)
        {
            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
            {
                ctx.Response.WithHeader("Access-Control-Allow-Origin", "*");

                string allowHeader = ctx.Response.Headers.GetOrDefault("Allow");
                if (allowHeader != null)
                    ctx.Response.WithHeader("Access-Control-Allow-Methods", allowHeader);
            });
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
    }
}