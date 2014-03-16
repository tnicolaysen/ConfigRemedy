using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Raven.Client;
using Raven.Client.Embedded;

namespace ConfigRemedy.Controllers
{
    public abstract class RavenDbController : ApiController
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

        public IDocumentStore Store
        {
            get { return LazyDocStore.Value; }
        }

        public IAsyncDocumentSession Session { get; set; }

        public override async Task<HttpResponseMessage> ExecuteAsync(
            HttpControllerContext controllerContext,
            CancellationToken cancellationToken)
        {
            using (Session = Store.OpenAsyncSession())
            {
                HttpResponseMessage result = await base.ExecuteAsync(controllerContext, cancellationToken);
                await Session.SaveChangesAsync();

                return result;
            }
        }
    }
}