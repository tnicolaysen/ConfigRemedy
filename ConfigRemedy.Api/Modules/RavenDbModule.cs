using System;
using Nancy;
using Raven.Client;
using Raven.Client.Embedded;

namespace ConfigRemedy.Api.Modules
{
    public abstract class RavenDbModule : NancyModule
    {
        private static readonly Lazy<IDocumentStore> LazyDocStore = new Lazy<IDocumentStore>(() =>
        {
            var docstore = new EmbeddableDocumentStore
            {
                DataDirectory = "Data"
            };

            docstore.Initialize();
            return docstore;
        });

        protected RavenDbModule()
        {
        }

        protected RavenDbModule(string modulePath) : base(modulePath)
        {
        }

        public IDocumentStore Store
        {
            get { return LazyDocStore.Value; }
        }

        public IDocumentSession DbSession { get; set; }

        //public override async Task<HttpResponseMessage> ExecuteAsync(
        //    HttpControllerContext controllerContext,
        //    CancellationToken cancellationToken)
        //{
        //    using (Session = Store.OpenAsyncSession())
        //    {
        //        HttpResponseMessage result = await base.ExecuteAsync(controllerContext, cancellationToken);
        //        await Session.SaveChangesAsync();

        //        return result;
        //    }
        //}
    }
}