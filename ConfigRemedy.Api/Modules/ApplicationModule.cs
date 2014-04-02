using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Raven.Client;

namespace ConfigRemedy.Api.Modules
{
    public class ApplicationModule :NancyModule
    {
        public ApplicationModule(IDocumentStore docStore)
            : base("/environments/{envName}/applications")
        {
            Get["/"] = _ =>
            {
                return _.envName ?? "Haha";
            };
        }
    }
}