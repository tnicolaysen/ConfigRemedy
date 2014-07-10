using ConfigRemedy.Api.Annotations;
using Nancy;

namespace ConfigRemedy.Api.Modules
{
    [UsedImplicitly]
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = parameters =>
            {
                return View["index"];
            };
        }
    }
}