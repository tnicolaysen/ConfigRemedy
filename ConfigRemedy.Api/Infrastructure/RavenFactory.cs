﻿using ConfigRemedy.Core.Configuration.Settings;
using Raven.Client;
using Raven.Client.Embedded;

namespace ConfigRemedy.Api.Infrastructure
{
    public static class RavenFactory
    {
        public static IDocumentStore Create()
        {
            var docstore = new EmbeddableDocumentStore
            {
                DataDirectory = Settings.DbPath
            };

            RavenConfigurator.Configure(docstore);

            docstore.Initialize();
            return docstore;
        }
    }
}