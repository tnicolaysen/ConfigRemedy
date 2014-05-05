using ConfigRemedy.Api.Infrastructure;
using Nancy.TinyIoc;
using Raven.Client;

namespace ConfigRemedy.Api
{
    using Nancy;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
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