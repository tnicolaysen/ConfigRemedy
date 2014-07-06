using Nancy;
using Nancy.Bootstrapper;
using Raven.Abstractions.Extensions;

namespace ConfigRemedy.Api.Infrastructure
{
    public static class CustomPipelines
    {
        public static void Configure(IPipelines pipelines)
        {
            AddCrossOriginSupport(pipelines);
        }

        private static void AddCrossOriginSupport(IPipelines pipelines)
        {
            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
            {
                ctx.Response.WithHeader("Access-Control-Allow-Origin", "*");
                ctx.Response.WithHeader("Access-Control-Allow-Headers", "origin, accept, content-type");

                string allowHeader = ctx.Response.Headers.GetOrDefault("Allow");
                if (allowHeader != null)
                    ctx.Response.WithHeader("Access-Control-Allow-Methods", allowHeader);
            });
        }
    }
}
